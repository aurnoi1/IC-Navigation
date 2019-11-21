using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Enums;
using IC.Navigation.Interfaces;
using IC.TimeoutEx;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace IC.Navigation.UnitTests
{
    public class NavigableExTests
    {
        [Fact]
        public void Exist_Should_Returns_True()
        {
            // Arrange
            var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
            Mock.Get(sut).Setup(x => x.PublishStatus().Exist)
                .Returns(new State<bool>(sut, StatesNames.Exist, true));

            // Act
            var actual = sut.Exists();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Exist_Should_Returns_False()
        {
            // Arrange
            var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
            Mock.Get(sut).Setup(x => x.PublishStatus().Exist)
                .Returns(new State<bool>(sut, StatesNames.Exist, false));

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

        private void SetNavigableSessionForDoTests(
            INavigable origin,
            Action<CancellationToken> action,
            CancellationToken globalCancellationToken,
            CancellationToken localCancellationToken)
        {
            var sessionMock = new Mock<NavigatorSession>();
            sessionMock.CallBase = true;
            sessionMock.Object.GlobalCancellationToken = globalCancellationToken;
            Mock.Get(origin).Setup(x => x.NavigatorSession).Returns(sessionMock.Object);
            Mock.Get(origin).Setup(x => x.NavigatorSession.GlobalCancellationToken).Returns(globalCancellationToken);
            Mock.Get(origin).Setup(x => x.NavigatorSession.Do(origin, action, localCancellationToken)).Returns(origin);
        }

        private void SetNavigableSessionForDoTests(
            INavigable origin,
            Func<CancellationToken, INavigable> action,
            CancellationToken globalCancellationToken,
            CancellationToken localCancellationToken)
        {
            var sessionMock = new Mock<NavigatorSession>();
            sessionMock.CallBase = true;
            sessionMock.Object.GlobalCancellationToken = globalCancellationToken;
            Mock.Get(origin).Setup(x => x.NavigatorSession).Returns(sessionMock.Object);
            Mock.Get(origin).Setup(x => x.NavigatorSession.GlobalCancellationToken).Returns(globalCancellationToken);
            Mock.Get(origin).Setup(x => x.NavigatorSession.Do<INavigable>(origin, action, localCancellationToken)).Returns(origin);
        }

        private void SetNavigableSessionForGoToTests(
            INavigable origin,
            INavigable destination,
            CancellationToken globalCancellationToken,
            CancellationToken localCancellationToken)
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
            Mock.Get(origin).Setup(x => x.NavigatorSession).Returns(sessionMock.Object);
            Mock.Get(origin).Setup(x => x.NavigatorSession.GlobalCancellationToken).Returns(globalCancellationToken);
            Mock.Get(origin).Setup(x => x.NavigatorSession.GoTo(origin, destination, localCancellationToken)).Returns(origin);
        }

        private void SetNavigableSessionForBackTests(
            INavigable origin,
            CancellationToken globalCancellationToken,
            CancellationToken localCancellationToken)
        {
            var sessionMock = new Mock<NavigatorSession>();
            Mock.Get(origin).Setup(x => x.NavigatorSession).Returns(sessionMock.Object);
            Mock<IGraph> iGraph = new Mock<IGraph>();
            var path = new Fixture().Customize(new AutoMoqCustomization()).Create<List<INavigable>>();
            var previous = path.First();
            path.Insert(1, origin);
            iGraph.Setup(g => g.GetShortestPath(origin, previous)).Returns(new List<INavigable>() { origin, previous });
            sessionMock.CallBase = true;
            sessionMock.Object.GlobalCancellationToken = globalCancellationToken;
            sessionMock.SetupGet(x => x.Graph).Returns(iGraph.Object);
            Mock.Get(origin).Setup(x => x.NavigatorSession.Last).Returns(origin);
            Mock.Get(origin).Setup(x => x.NavigatorSession.Previous).Returns(previous);
            Mock.Get(origin).Setup(x => x.NavigatorSession.GlobalCancellationToken).Returns(globalCancellationToken);
            Mock.Get(origin).Setup(x => x.NavigatorSession.Back(localCancellationToken)).Returns(previous);
        }

        #region Back Tests

        [Fact]
        public void Back_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(1000.ms());
            using var globalCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            SetNavigableSessionForBackTests(origin, globalCts.Token, default);

            // Act
            INavigable sut() => origin.Back();

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Back().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Back(It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Back_Should_Be_Interrupted_By_LocalCancellationToken_Before_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(3000.ms());
            using var globalCts = new CancellationTokenSource(2000.ms());
            using var localCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSessionForBackTests(origin, globalCts.Token, localCts.Token);

            // Act
            INavigable sut() => origin.Back(localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.False(globalCts.IsCancellationRequested);
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.Back().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Back(It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Back_Should_Be_Interrupted_By_GlobalCancellationToken_Before_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(3000.ms());
            using var globalCts = new CancellationTokenSource(250.ms());
            using var localCts = new CancellationTokenSource(2000.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSessionForBackTests(origin, globalCts.Token, localCts.Token);

            // Act
            INavigable sut() => origin.Back(localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(globalCts.IsCancellationRequested);
            Assert.False(localCts.IsCancellationRequested);
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.Back().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Back(It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Back_Should_Be_Interrupted_By_LocalCancellationToken_When_No_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(3000.ms());
            using var localCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSessionForBackTests(origin, default, localCts.Token);

            // Act
            INavigable sut() => origin.Back(localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(default, origin.NavigatorSession.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.Back().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Back(It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        #endregion Back Tests

        #region Goto Tests

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(1000.ms());
            using var globalCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSessionForGoToTests(origin, destination, globalCts.Token, default);

            // Act
            INavigable sut() => origin.GoTo(destination);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.GoTo().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.GoTo(origin, It.IsAny<INavigable>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_LocalCancellationToken_Before_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(1000.ms());
            using var globalCts = new CancellationTokenSource(3000.ms());
            using var localCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSessionForGoToTests(origin, destination, globalCts.Token, localCts.Token);

            // Act
            INavigable sut() => origin.GoTo(destination, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.False(globalCts.IsCancellationRequested);
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.GoTo().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.GoTo(origin, It.IsAny<INavigable>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_GlobalCancellationToken_Before_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(1000.ms());
            using var globalCts = new CancellationTokenSource(250.ms());
            using var localCts = new CancellationTokenSource(3000.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSessionForGoToTests(origin, destination, globalCts.Token, localCts.Token);

            // Act
            INavigable sut() => origin.GoTo(destination, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(globalCts.IsCancellationRequested);
            Assert.False(localCts.IsCancellationRequested);
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.GoTo().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.GoTo(origin, It.IsAny<INavigable>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_LocalCancellationToken_When_No_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(1000.ms());
            using var localCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            SetNavigableSessionForGoToTests(origin, destination, default, localCts.Token);

            // Act
            INavigable sut() => origin.GoTo(destination, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(default, origin.NavigatorSession.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.GoTo().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.GoTo(origin, It.IsAny<INavigable>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        #endregion Goto Tests

        #region Do<INavigable> Tests

        [Fact]
        public void Do_With_Cancellable_Function_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(1000.ms());
            using var globalCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            Func<CancellationToken, INavigable> function = GetCancellableFunction(testTimeout.Token, destination);
            SetNavigableSessionForDoTests(origin, function, globalCts.Token, default);

            // Act
            void sut() => origin.Do<INavigable>(function);

            //Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do<INavigable>().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Do<INavigable>(origin, It.IsAny<Func<CancellationToken, INavigable>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Function_Should_Be_Interrupted_By_LocalCancellationToken_Before_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(5000.ms());
            using var globalCts = new CancellationTokenSource(3000.ms());
            using var localCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            Func<CancellationToken, INavigable> function = GetCancellableFunction(testTimeout.Token, destination);
            SetNavigableSessionForDoTests(origin, function, globalCts.Token, localCts.Token);

            // Act
            void sut() => origin.Do<INavigable>(function, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);
            Assert.False(globalCts.IsCancellationRequested);
            Assert.True(localCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do<INavigable>().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Do<INavigable>(origin, It.IsAny<Func<CancellationToken, INavigable>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Function_Should_Be_Interrupted_By_GlobalCancellationToken_Before_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(5000.ms());
            using var globalCts = new CancellationTokenSource(250.ms());
            using var localCts = new CancellationTokenSource(3000.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            Func<CancellationToken, INavigable> function = GetCancellableFunction(testTimeout.Token, destination);
            SetNavigableSessionForDoTests(origin, function, globalCts.Token, localCts.Token);

            // Act
            void sut() => origin.Do<INavigable>(function, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);
            Assert.False(localCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do<INavigable>().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Do<INavigable>(origin, It.IsAny<Func<CancellationToken, INavigable>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Function_Should_Be_Interrupted_By_LocalCancellationToken_When_No_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(5000.ms());
            using var localCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            var destination = fixture.Create<INavigable>();
            Func<CancellationToken, INavigable> function = GetCancellableFunction(testTimeout.Token, destination);
            SetNavigableSessionForDoTests(origin, function, default, localCts.Token);

            // Act
            void sut() => origin.Do<INavigable>(function, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(default, origin.NavigatorSession.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.Do<INavigable>().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Do<INavigable>(origin, It.IsAny<Func<CancellationToken, INavigable>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        #endregion Do<INavigable> Tests

        #region Do Tests

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(1000.ms());
            using var globalCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            Action<CancellationToken> action = GetCancellableAction(testTimeout.Token);
            SetNavigableSessionForDoTests(origin, action, globalCts.Token, default);

            // Act
            void sut() => origin.Do(action);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Do(origin, It.IsAny<Action<CancellationToken>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_LocalCancellationToken_Before_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(3000.ms());
            using var globalCts = new CancellationTokenSource(2000.ms());
            using var localCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            Action<CancellationToken> action = GetCancellableAction(testTimeout.Token);
            SetNavigableSessionForDoTests(origin, action, globalCts.Token, localCts.Token);

            // Act
            void sut() => origin.Do(action, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);
            Assert.True(localCts.IsCancellationRequested);
            Assert.False(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Do(origin, It.IsAny<Action<CancellationToken>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_GlobalCancellationToken_Before_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(3000.ms());
            using var globalCts = new CancellationTokenSource(250.ms());
            using var localCts = new CancellationTokenSource(2000.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            Action<CancellationToken> action = GetCancellableAction(testTimeout.Token);
            SetNavigableSessionForDoTests(origin, action, globalCts.Token, localCts.Token);

            // Act
            void sut() => origin.Do(action, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.Equal(globalCts.Token, origin.NavigatorSession.GlobalCancellationToken);
            Assert.False(localCts.IsCancellationRequested);
            Assert.True(globalCts.IsCancellationRequested);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Do(origin, It.IsAny<Action<CancellationToken>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_LocalCancellationToken_When_No_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(3000.ms());
            using var localCts = new CancellationTokenSource(250.ms());
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            var origin = fixture.Create<INavigable>();
            Action<CancellationToken> action = GetCancellableAction(testTimeout.Token);
            SetNavigableSessionForDoTests(origin, action, default, localCts.Token);

            // Act
            void sut() => origin.Do(action, localCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut());
            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(default, origin.NavigatorSession.GlobalCancellationToken);

            // Ensure task was not cancelled before to call Session.Do().
            Mock.Get(origin)
                .Verify(x =>
                    x.NavigatorSession.Do(origin, It.IsAny<Action<CancellationToken>>(), It.IsAny<CancellationToken>()),
                    Times.Once
                );
        }

        #endregion Do Tests
    }
}