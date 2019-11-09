using IC.Navigation.Interfaces;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using TechTalk.SpecFlow;
using IC.Navigation.CoreExtensions;
using System.Drawing;
using System.IO;
using ApprovalTests;
using OpenQA.Selenium;
using ApprovalTests.Namers;

namespace IC.Navigation.UITests.Specflow.Steps
{
    [Binding]
    public class AppUISteps
    {
        private readonly IAppBrowser<WindowsDriver<WindowsElement>> browser;

        private AppUISteps(AppiumContext<WindowsDriver<WindowsElement>> appiumContext)
        {
            browser = appiumContext.Browser;
        }

        [When(@"I save a picture of ""(.*)"" as ""(.*)""")]
        public void WhenISaveAPictureOfAs(INavigable page, string expectedImageName)
        {
            browser.Last
                .GoTo(page)
                .Do(() =>
                {
                    var screenshot = browser.RemoteDriver.GetScreenshot();
                    var pagePath = $"{expectedImageName}.png";
                    screenshot.SaveAsFile(pagePath, ScreenshotImageFormat.Png);
                               
                });
        }

        [Then(@"the picture ""(.*)"" should match the approved one")]
        public void ThenThePictureShouldMatchTheApprovedOne(string expectedImageName)
        {
            var pagePath = $"{expectedImageName}.png";
            using (ApprovalResults.ForScenario(expectedImageName))
            {
                Approvals.VerifyFile(pagePath);
            }
        }
    }
}
