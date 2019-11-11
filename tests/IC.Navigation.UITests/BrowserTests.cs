using IC.Navigation.Interfaces;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium;
using IC.Tests.App.Poms.Appium.POMs;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class BrowserTests : IDisposable
    {
        public BrowserTests()
        {
            appContext = new WindowsContext<WindowsDriver<WindowsElement>>();
        }

        #region Properties

        #region Private

        private readonly WindowsContext<WindowsDriver<WindowsElement>> appContext;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void Constructor_Should_Set_GlobalCancellationToken()
        {
            //Arrange
            var appiumSession = appContext.CreateAppAppiumSession();
            using var cts = new CancellationTokenSource();

            // Act
            using AppBrowser<WindowsDriver<WindowsElement>> appBrowser = new AppBrowser<WindowsDriver<WindowsElement>>(appiumSession, cts.Token);

            // Assert
            Assert.Equal(cts.Token, appBrowser.GlobalCancellationToken);
        }

        [Fact]
        public void WaitForEntryPoints_With_CToken_Should_Returns_Found_EntryPoint()
        {
            var expected = typeof(PomMenu<WindowsDriver<WindowsElement>>);
            INavigable actual = null;
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            using (var appBrowser = appContext.CreateAppBrowser())
            {
                actual = appBrowser.WaitForEntryPoints(cts.Token);
            }

            Assert.Equal(expected, actual.GetType());
        }

        [Fact]
        public void WaitForEntryPoints_With_CToken_Should_Throw_OperationCanceledException_On_Timeout()
        {
            using var cts = new CancellationTokenSource(TimeSpan.Zero);
            using var appBrowser = appContext.CreateAppBrowser();
            Assert.Throws<OperationCanceledException>(() => appBrowser.WaitForEntryPoints(cts.Token));
        }

        [Fact]
        public void WaitForEntryPoints_With_Timeout_Should_Returns_Found_EntryPoint()
        {
            var expected = typeof(PomMenu<WindowsDriver<WindowsElement>>);
            INavigable actual = null;
            using (var appBrowser = appContext.CreateAppBrowser())
            {
                actual = appBrowser.WaitForEntryPoints(TimeSpan.FromSeconds(10));
            }

            Assert.Equal(expected, actual.GetType());
        }

        [Fact]
        public void WaitForEntryPoints_With_Timeout_Should_Throw_TimeoutException_On_Timeout()
        {
            using var appBrowser = appContext.CreateAppBrowser();
            Assert.Throws<TimeoutException>(() => appBrowser.WaitForEntryPoints(TimeSpan.Zero));
        }

        public void Dispose()
        {
            appContext.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}