﻿using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace IC.Navigation.UnitTests
{
    public class INavigableExTests
    {
        [Fact]
        public void Exists_Should_Returns_True()
        {
            // Arrange
            var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
            Mock.Get(sut).Setup(x => x.PublishStatus().Exists).Returns(true);

            // Act
            var actual = sut.Exists();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Exists_Should_Returns_False()
        {
            // Arrange
            var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
            Mock.Get(sut).Setup(x => x.PublishStatus().Exists).Returns(false);

            // Act
            var actual = sut.Exists();

            // Assert
            Assert.False(actual);
        }

        private Action<CancellationToken> GetCancellableAction(CancellationToken testTimeoutToken)
        {
            void action(CancellationToken ct)
            {
                while (!testTimeoutToken.IsCancellationRequested)
                {
                    // GlobalCancellationToken is set as ct in Do() extension method.
                    ct.ThrowIfCancellationRequested();
                    Thread.Sleep(150);
                }
            }

            return action;
        }

        private Func<CancellationToken, INavigable> GetCancellableFunction(CancellationToken testTimeoutToken, INavigable returnValue)
        {
            INavigable function(CancellationToken ct)
            {
                while (!testTimeoutToken.IsCancellationRequested)
                {
                    // GlobalCancellationToken is set as ct in Do() extension method.
                    ct.ThrowIfCancellationRequested();
                    Thread.Sleep(150);
                }

                return returnValue;
            }

            return function;
        }

        private void SetNavigableSession(
            INavigable origin,
            Action<CancellationToken> action,
            CancellationToken globalCancellationToken)
        {
            var sessionMock = new Mock<NavigatorSession>();
            sessionMock.CallBase = true;
            sessionMock.Object.GlobalCancellationToken = globalCancellationToken;
            Mock.Get(origin).Setup(x => x.Session).Returns(sessionMock.Object);
            Mock.Get(origin).Setup(x => x.Session.GlobalCancellationToken).Returns(globalCancellationToken);
            Mock.Get(origin).Setup(x => x.Session.Do(origin, action, CancellationToken.None)).Returns(origin);
        }

        private void SetNavigableSession(
            INavigable origin,
            Func<CancellationToken, INavigable> action,
            CancellationToken globalCancellationToken)
        {
            var sessionMock = new Mock<NavigatorSession>();
            sessionMock.CallBase = true;
            sessionMock.Object.GlobalCancellationToken = globalCancellationToken;
            Mock.Get(origin).Setup(x => x.Session).Returns(sessionMock.Object);
            Mock.Get(origin).Setup(x => x.Session.GlobalCancellationToken).Returns(globalCancellationToken);
            Mock.Get(origin).Setup(x => x.Session.Do<INavigable>(origin, action, CancellationToken.None)).Returns(origin);
        }

        private void SetNavigableSession(
            INavigable origin,
            INavigable destination,
            CancellationToken globalCancellationToken)
        {
            Mock<IGraph> iGraph = new Mock<IGraph>();
            var path = new Fixture().Customize(new AutoMoqCustomization()).Create<List<INavigable>>();
            path.Insert(0, origin);
            path.Insert(path.Count, destination);
            iGraph.Setup(g => g.GetShortestPath(origin, destination)).Returns(path);
            var sessionMock = new Mock<NavigatorSession>();
            sessionMock.CallBase = true;
            sessionMock.Object.GlobalCancellationToken = globalCancellationToken;
            sessionMock.SetupGet(x => x.Graph).Returns(iGraph.Object);
            Mock.Get(origin).Setup(x => x.Session).Returns(sessionMock.Object);
            Mock.Get(origin).Setup(x => x.Session.GlobalCancellationToken).Returns(globalCancellationToken);
            Mock.Get(origin).Setup(x => x.Session.GoTo(origin, destination, CancellationToken.None)).Returns(origin);
        }

        #region Goto Tests

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSession(origin, destination, globalCts.Token);

            // Act
            INavigable sut() => origin.GoTo(destination);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.GoTo().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.GoTo(origin, It.IsAny<INavigable>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_LocalCancellationToken_Before_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(3000));
            using var localCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSession(origin, destination, globalCts.Token);

            // Act
            INavigable sut() => origin.GoTo(destination, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.False(globalCts.IsCancellationRequested);
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.GoTo().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.GoTo(origin, It.IsAny<INavigable>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }


        [Fact]
        public void GoTo_Should_Be_Interrupted_By_GlobalCancellationToken_Before_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            using var localCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(3000));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSession(origin, destination, globalCts.Token);

            // Act
            INavigable sut() => origin.GoTo(destination, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(globalCts.IsCancellationRequested);
            Assert.False(localCts.IsCancellationRequested);
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.GoTo().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.GoTo(origin, It.IsAny<INavigable>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }


        [Fact]
        public void GoTo_Should_Be_Interrupted_By_LocalCancellationToken_When_No_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            using var localCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSession(origin, destination, default);

            // Act
            INavigable sut() => origin.GoTo(destination, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(default, origin.Session.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.GoTo().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.GoTo(origin, It.IsAny<INavigable>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        #endregion Goto Tests

        #region Do<INavigable> Tests

        [Fact]
        public void Do_With_Cancellable_Function_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            Func<CancellationToken, INavigable> function = GetCancellableFunction(testTimeout.Token, destination);
            SetNavigableSession(origin, function, globalCts.Token);

            // Act
            void sut() => origin.Do<INavigable>(function);

            //Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.Do<INavigable>(origin, It.IsAny<Func<CancellationToken, INavigable>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Function_Should_Be_Interrupted_By_LocalCancellationToken_Before_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(5000));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(3000));
            using var localCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            Func<CancellationToken, INavigable> function = GetCancellableFunction(testTimeout.Token, destination);
            SetNavigableSession(origin, function, globalCts.Token);

            // Act
            void sut() => origin.Do<INavigable>(function, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);
            Assert.False(globalCts.IsCancellationRequested);
            Assert.True(localCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.Do<INavigable>(origin, It.IsAny<Func<CancellationToken, INavigable>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Function_Should_Be_Interrupted_By_GlobalCancellationToken_Before_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(5000));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            using var localCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(3000));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            Func<CancellationToken, INavigable> function = GetCancellableFunction(testTimeout.Token, destination);
            SetNavigableSession(origin, function, globalCts.Token);

            // Act
            void sut() => origin.Do<INavigable>(function, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);
            Assert.False(localCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.Do<INavigable>(origin, It.IsAny<Func<CancellationToken, INavigable>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Function_Should_Be_Interrupted_By_LocalCancellationToken_When_No_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(5000));
            using var localCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            Func<CancellationToken, INavigable> function = GetCancellableFunction(testTimeout.Token, destination);
            SetNavigableSession(origin, function, default);

            // Act
            void sut() => origin.Do<INavigable>(function, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(default, origin.Session.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.Do<INavigable>(origin, It.IsAny<Func<CancellationToken, INavigable>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        #endregion Do<INavigable> Tests

        #region Do Tests

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            Action<CancellationToken> action = GetCancellableAction(testTimeout.Token);
            SetNavigableSession(origin, action, globalCts.Token);

            // Act
            void sut() => origin.Do(action);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.Do(origin, It.IsAny<Action<CancellationToken>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_LocalCancellationToken_Before_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(3000));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(2000));
            using var localCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            Action<CancellationToken> action = GetCancellableAction(testTimeout.Token);
            SetNavigableSession(origin, action, globalCts.Token);

            // Act
            void sut() => origin.Do(action, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);
            Assert.True(localCts.IsCancellationRequested);
            Assert.False(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.Do(origin, It.IsAny<Action<CancellationToken>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_GlobalCancellationToken_Before_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(3000));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            using var localCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(2000));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            Action<CancellationToken> action = GetCancellableAction(testTimeout.Token);
            SetNavigableSession(origin, action, globalCts.Token);

            // Act
            void sut() => origin.Do(action, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);
            Assert.False(localCts.IsCancellationRequested);
            Assert.True(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.Do(origin, It.IsAny<Action<CancellationToken>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_LocalCancellationToken_When_No_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(3000));
            using var localCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            Action<CancellationToken> action = GetCancellableAction(testTimeout.Token);
            SetNavigableSession(origin, action, default);

            // Act
            void sut() => origin.Do(action, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(default, origin.Session.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.Session.Do(origin, It.IsAny<Action<CancellationToken>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        #endregion Do Tests
    }
}