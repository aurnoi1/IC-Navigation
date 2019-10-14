using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.CoreExtensions;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using IC.Tests.App.Poms.Appium.POMs;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;
using System.Threading.Tasks;
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

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            sut.GlobalCancellationToken = globalCts.Token;
            
            // Act, Assert
            Assert.Throws<OperationCanceledException>(() =>
                sut.Last
                    .Do((ct) =>
                    {
                        while (!testTimeout.IsCancellationRequested)
                        {
                            // GlobalCancellationToken is set as ct in Do() extension method.
                            ct.ThrowIfCancellationRequested();
                            Thread.Sleep(1000);
                        }
                    })
            );

            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(globalCts.IsCancellationRequested);
        }

        [Fact]
        public void Do_With_Cancellable_Action_Should_Be_Interrupted_By_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var localCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            sut.GlobalCancellationToken = globalCts.Token;

            // Act, Assert
            Assert.Throws<OperationCanceledException>(() =>
                sut.Last
                    .Do((ct) =>
                    {
                        while (!testTimeout.IsCancellationRequested)
                        {
                            // ct should be the linked token of GlobalCancellationToken and localCts in Do() extension method.
                            ct.ThrowIfCancellationRequested();
                            Thread.Sleep(1000);
                        }
                    }, localCts.Token)
            );

            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.False(globalCts.IsCancellationRequested);
        }


        [Fact]
        public void Do_Returning_INavigable_With_Cancellable_Action_Should_Be_Interrupted_By_GlobalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            sut.GlobalCancellationToken = globalCts.Token;

            // Act, Assert
            Assert.Throws<OperationCanceledException>(() =>
                sut.Last
                    .Do<PomMenu>((ct) =>
                    {
                        while (!testTimeout.IsCancellationRequested)
                        {
                            // GlobalCancellationToken is set as ct in Do() extension method.
                            ct.ThrowIfCancellationRequested();
                            Thread.Sleep(1000);
                        }

                        return sut.PomMenu;
                    })
            );

            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.True(globalCts.IsCancellationRequested);
        }

        [Fact]
        public void Do_Returning_INavigable_With_Cancellable_Action_Should_Be_Interrupted_By_LocalCancellationToken()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            using var globalCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var localCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            sut.GlobalCancellationToken = globalCts.Token;

            // Act, Assert
            Assert.Throws<OperationCanceledException>(() =>
                sut.Last
                    .Do<PomMenu>((ct) =>
                    {
                        while (!testTimeout.IsCancellationRequested)
                        {
                            // ct should be the linked token of GlobalCancellationToken and localCts in Do() extension method.
                            ct.ThrowIfCancellationRequested();
                            Thread.Sleep(1000);
                        }

                        return sut.PomMenu;
                    }, localCts.Token)
            );

            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
            Assert.False(globalCts.IsCancellationRequested);
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}