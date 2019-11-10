using ApprovalTests;
using ApprovalTests.Namers;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;

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
                    using var ms = new MemoryStream(screenshot.AsByteArray);
                    using var bmp = new Bitmap(ms);
                    using var crop = TrimImage(bmp, 5);
                    var pagePath = $"{expectedImageName}.png";
                    bmp.Save(pagePath, ImageFormat.Png);
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

        private Bitmap TrimImage(Bitmap image, int trimPixelCount)
        {
            Bitmap retVal = new Bitmap(image.Width, image.Height);
            Rectangle boudingRectangle = new Rectangle(0, 0, 
                image.Width - trimPixelCount, 
                image.Height - trimPixelCount);

            Graphics graphic = Graphics.FromImage(retVal);
            graphic.DrawImage(image, -boudingRectangle.X, -boudingRectangle.Y);
            return retVal;
        }
    }
}