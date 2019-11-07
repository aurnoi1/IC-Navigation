using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Navigation.UITests.Specflow.StepArgumentTransformations;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace IC.Navigation.UITests.Specflow.Steps
{
    [Binding]
    [Collection("UITests")]
    public class ControlSteps : IDisposable
    {
        private readonly IBrowser<WindowsDriver<WindowsElement>> sut;

        private ControlSteps(AppiumContext<WindowsDriver<WindowsElement>> appiumContext)
        {
            this.sut = appiumContext.SUT;
        }

        [Then(@"The control ""(.*)"" should be displayed in the current page")]
        public void ThenTheControlShouldBeDisplayed(WindowsElement control)
        {
            Assert.True(control.Displayed, $"The control {control.Id} was not displayed.");
        }

        [Then(@"The control ""(.*)"" should not be displayed in the current page")]
        public void ThenTheControlWithUsageNameShouldNotBeDisplayed(WindowsElement control)
        {
            Assert.Null(control);
        }

        [Then(@"The following controls should be displayed in the current page:")]
        public void ThenTheFollowingControlsShouldBeDisplayedInTheCurrentPage(Table table)
        {
            var usageNameCol = table.Rows.Where(x => x.Keys.Single().Equals("usage_name"));
            foreach (var value in usageNameCol)
            {
                var control = sut.FindElementByAliasesInLastINavigable(value.Values.FirstOrDefault());
                ThenTheControlShouldBeDisplayed(control);
            }
        }

        public void Dispose()
        {
            sut?.Dispose();
        }
    }
}