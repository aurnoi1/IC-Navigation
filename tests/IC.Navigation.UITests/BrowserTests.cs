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
    public class BrowserTests
    {
        public BrowserTests()
        {
            appContext = new AppiumContext<WindowsDriver<WindowsElement>>();
        }

        #region Properties

        #region Private

        private readonly AppiumContext<WindowsDriver<WindowsElement>> appContext;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void Constructor_Should_Set_GlobalCancellationToken()
        {
            //Arrange
            var appiumSession = appContext.GetAppiumSession();
            using var cts = new CancellationTokenSource();

            // Act
            using AppBrowser<WindowsDriver<WindowsElement>> facade = new AppBrowser<WindowsDriver<WindowsElement>>(appiumSession, cts.Token);

            // Assert
            Assert.Equal(cts.Token, facade.GlobalCancellationToken);
        }

        [Fact]
        public void WaitForEntryPoints_With_CToken_Should_Returns_Found_EntryPoint()
        {
            var expected = typeof(PomMenu<WindowsDriver<WindowsElement>>);
            INavigable actual = null;
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            using (var sut = appContext.GetAppBrowser())
            {
                actual = sut.WaitForEntryPoints(cts.Token);
            }

            Assert.Equal(expected, actual.GetType());
        }

        [Fact]
        public void WaitForEntryPoints_With_CToken_Should_Throw_OperationCanceledException_On_Timeout()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.Zero))
            using (var sut = appContext.GetAppBrowser())
            {
                Assert.Throws<OperationCanceledException>(() => sut.WaitForEntryPoints(cts.Token));
            }
        }

        [Fact]
        public void WaitForEntryPoints_With_Timeout_Should_Returns_Found_EntryPoint()
        {
            var expected = typeof(PomMenu<WindowsDriver<WindowsElement>>);
            INavigable actual = null;
            using (var sut = appContext.GetAppBrowser())
            {
                actual = sut.WaitForEntryPoints(TimeSpan.FromSeconds(10));
            }

            Assert.Equal(expected, actual.GetType());
        }

        [Fact]
        public void WaitForEntryPoints_With_Timeout_Should_Throw_TimeoutException_On_Timeout()
        {
            using (var sut = appContext.GetAppBrowser())
            {
                Assert.Throws<TimeoutException>(() => sut.WaitForEntryPoints(TimeSpan.Zero));
            }
        }

        #endregion Public

        #endregion Methods
    }
}