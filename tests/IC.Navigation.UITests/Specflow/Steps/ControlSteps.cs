using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
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
        private IFacade sut;

        private ControlSteps(AppiumContext appiumContext)
        {
            this.sut = appiumContext.SUT;
        }

        [Then(@"The control ""(.*)"" should be displayed in the current page")]
        public void ThenTheControlShouldBeDisplayed(string usageName)
        {
            WindowsElement match = sut.FindElementByAliasesInLastINavigable(usageName);
            Assert.True(match.Displayed, $"The control with usage name {usageName} was not displayed.");
        }

        [Then(@"The control ""(.*)"" should not be displayed in the current page")]
        public void ThenTheControlWithUsageNameShouldNotBeDisplayed(string usageName)
        {
            WindowsElement match = sut.FindElementByAliasesInLastINavigable(usageName);
            Assert.Null(match);
        }

        [Then(@"The following controls should be displayed in the current page:")]
        public void ThenTheFollowingControlsShouldBeDisplayedInTheCurrentPage(Table table)
        {
            var usageNameCol = table.Rows.Where(x => x.Keys.Single().Equals("usage_name"));
            foreach (var value in usageNameCol)
            {
                ThenTheControlShouldBeDisplayed(value.Values.FirstOrDefault());
            }
        }

        [Given(@"(.*) day from today in the news paper")]
        public void GivenDayFromToday(DateTime date)
        {
            var d = date;
        }

        [Given(@"(.*) day\(s\) in the news paper")]
        public void GivenDaySInTheNewsPaper(DateTime date)
        {
            var d = date;
        }

        [Given(@"(.*) apple in the basket")]
        public void GivenAppleInTheBasket(int num)
        {
            var i = num;
        }

        public void Dispose()
        {
            sut?.Dispose();
        }
    }
}