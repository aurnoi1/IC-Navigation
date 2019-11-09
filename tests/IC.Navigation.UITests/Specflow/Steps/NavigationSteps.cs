using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;

namespace IC.Navigation.UITests.Specflow.Steps
{
    [Binding]
    [Collection("UITests")]
    public class NavigationSteps : IDisposable
    {
        private readonly IAppBrowser<WindowsDriver<WindowsElement>> browser;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken ct;

        public NavigationSteps(AppiumContext<WindowsDriver<WindowsElement>> appiumContext)
        {
            browser = appiumContext.Browser;
            cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            ct = cts.Token;
        }

        [Given(@"The application under test has been started")]
        public void GivenTheApplicationUnderTestHasBeenStarted()
        {
            Assert.NotNull(browser.Last);
        }

        [When(@"I navigate to ""(.*)""")]
        [Given(@"The ""(.*)"" has been opened")]
        public void GivenTheViewHasBeenOpened(INavigable destination)
        {
            browser.Last.GoTo(destination, ct);
        }

        [When(@"The ""(.*)"" is pressed in current page")]
        public void WhenIsPressedInCurrentView(WindowsElement control)
        {
            control.Click();
        }

        [Then(@"The ""(.*)"" should be opened")]
        public void ThenTheShouldBeOpened(INavigable expectedPage)
        {
            Assert.True(expectedPage.Exists());
        }

        [Then(@"The control ""(.*)"" should not be displayed")]
        public void ThenTheControlShouldNotBeDisplayed(WindowsElement control)
        {
            Assert.Null(control);
        }

        public void Dispose()
        {
            browser?.Dispose();
            cts?.Dispose();
        }
    }
}