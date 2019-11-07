using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium.WindowsDriver;
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
        private readonly IAppBrowser<WindowsDriver<WindowsElement>> sut;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken ct;

        public NavigationSteps(AppiumContext<WindowsDriver<WindowsElement>> appiumContext)
        {
            this.sut = appiumContext.SUT;
            cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            ct = cts.Token;
        }

        [Given(@"The application under test has been started")]
        public void GivenTheApplicationUnderTestHasBeenStarted()
        {
            Assert.NotNull(sut.Last);
        }

        [When(@"I navigate to ""(.*)""")]
        [Given(@"The ""(.*)"" has been opened")]
        public void GivenTheViewHasBeenOpened(INavigable destination)
        {
            sut.Last.GoTo(destination, ct);
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
            sut?.Dispose();
            cts?.Dispose();
        }
    }
}