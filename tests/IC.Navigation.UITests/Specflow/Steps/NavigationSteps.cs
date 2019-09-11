using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace IC.Navigation.UITests.Specflow.Steps
{
    [Binding]
    [Collection("UITests")]
    public class NavigationSteps : IDisposable
    {
        private readonly IFacade sut;

        public NavigationSteps(AppiumContext appiumContext)
        {
            this.sut = appiumContext.SUT;
        }

        [Given(@"The application under test has been started")]
        public void GivenTheApplicationUnderTestHasBeenStarted()
        {
            Assert.NotNull(sut.Last);
        }

        [Given(@"The ""(.*)"" has been opened")]
        public void GivenTheViewHasBeenOpened(string usageName)
        {
            var destination = sut.GetINavigableByUsageName(usageName);
            sut.Last.GoTo(destination);
        }

        [When(@"I navigate to ""(.*)""")]
        public void WhenINavigateTo(string usageName)
        {
            GivenTheViewHasBeenOpened(usageName);
        }

        [When(@"The ""(.*)"" is pressed in current page")]
        public void WhenIsPressedInCurrentView(string usageName)
        {
            WindowsElement match = sut.FindElementByAliasesInLastINavigable(usageName);
            match.Click();
        }

        [Then(@"The ""(.*)"" should be opened")]
        public void ThenTheShouldBeOpened(string viewUsageName)
        {
            var expectedView = sut.GetINavigableByUsageName(viewUsageName);
            Assert.True(expectedView.Exists());
        }

        [Then(@"The control ""(.*)"" should not be displayed")]
        public void ThenTheControlShouldNotBeDisplayed(string usageName)
        {
            var control = sut.FindElementByAliasesInLastINavigable(usageName);
            Assert.Null(control);
        }

        public void Dispose()
        {
            sut?.Dispose();
        }
    }
}