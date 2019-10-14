using AutoFixture;
using IC.Navigation.CoreExtensions;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using IC.Tests.App.Poms.Appium.POMs;
using System;
using System.Threading;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class NavigatorSessionExTests : IDisposable
    {
        public NavigatorSessionExTests()
        {
            sut = new AppiumContext().SUT;
        }

        #region Fields

        private readonly IFacade sut;

        #endregion Fields

        #region Methods

        #region Public

        #region Goto Tests

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            sut.GlobalCancellationToken = globalCts.Token;

            Assert.Throws<OperationCanceledException>(() =>
            {
                sut.Last.GoTo(sut.PomBlue);
                while (!testTimeout.IsCancellationRequested)
                {
                    sut.Last.GoTo(sut.Previous);
                }
            });

            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(sut.GlobalCancellationToken.IsCancellationRequested);
        }

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_LocalCancellationToken_Before_GlobalCancellationToken()
        {
                // Arrange
                using var testTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                using var globalCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                using var localCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
                sut.GlobalCancellationToken = globalCts.Token;

                Assert.Throws<OperationCanceledException>(() =>
                {
                    sut.Last.GoTo(sut.PomBlue, localCts.Token);
                    while (!testTimeout.IsCancellationRequested)
                    {
                        // ct should be the linked token of GlobalCancellationToken and localCts in GoTo() extension method.
                        sut.Last.GoTo(sut.Previous, localCts.Token);
                    }
                });

                Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
                Assert.True(localCts.IsCancellationRequested);
                Assert.False(sut.GlobalCancellationToken.IsCancellationRequested);
        }

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_GlobalCancellationToken_Before_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            using var localCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            sut.GlobalCancellationToken = globalCts.Token;

            Assert.Throws<OperationCanceledException>(() =>
            {
                sut.Last.GoTo(sut.PomBlue, localCts.Token);
                while (!testTimeout.IsCancellationRequested)
                {
                    // ct should be the linked token of GlobalCancellationToken and localCts in GoTo() extension method.
                    sut.Last.GoTo(sut.Previous, localCts.Token);
                }
            });

            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.False(localCts.IsCancellationRequested);
            Assert.True(sut.GlobalCancellationToken.IsCancellationRequested);
        }

        [Fact]
        public void GoTo_Should_Be_Interrupted_By_LocalCancellationToken_When_No_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            using var localCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

            Assert.Throws<OperationCanceledException>(() =>
            {
                sut.Last.GoTo(sut.PomBlue, localCts.Token);
                while (!testTimeout.IsCancellationRequested)
                {
                    // ct should be the linked token of GlobalCancellationToken and localCts in GoTo() extension method.
                    sut.Last.GoTo(sut.Previous, localCts.Token);
                }
            });

            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(localCts.IsCancellationRequested);
            Assert.Equal(default, sut.GlobalCancellationToken);
        }

        #endregion Goto Tests

        public void Dispose()
{
    sut?.Dispose();
}

        #endregion Public

        #endregion Methods
    }
}