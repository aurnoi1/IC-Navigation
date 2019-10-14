using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using Moq;
using System;
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
           Action<CancellationToken> action = (ct) =>
           {
               while (!testTimeoutToken.IsCancellationRequested)
               {
                    // GlobalCancellationToken is set as ct in Do() extension method.
                    ct.ThrowIfCancellationRequested();
                   Thread.Sleep(150);
               }
           };

            return action;
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

            // Act, Assert
            Assert.Throws<OperationCanceledException>(() => origin.Do(action));

            // Assert
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

            // Act, Assert
            Assert.Throws<OperationCanceledException>(() => origin.Do(action, localCts.Token));

            // Assert
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

            // Act, Assert
            Assert.Throws<OperationCanceledException>(() => origin.Do(action, localCts.Token));

            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
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

            // Act, Assert
            Assert.Throws<OperationCanceledException>(() => origin.Do(action, localCts.Token));

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