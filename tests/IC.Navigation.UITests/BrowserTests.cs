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
            using AppBrowser<WindowsDriver<WindowsElement>> facade = new AppBrowser<WindowsDriver<WindowsElement>>(appiumSession, cts.Token);

            // Assert
            Assert.Equal(cts.Token, facade.GlobalCancellationToken);
        }

        #endregion Public

        #endregion Methods
    }
}