using ApprovalTests;
using ApprovalTests.Namers;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TechTalk.SpecFlow;

namespace IC.Navigation.UITests.Specflow.Steps
{
    [Binding]
    public class AppUISteps
    {
        private readonly IAppBrowser<WindowsDriver<WindowsElement>> browser;
        private readonly WindowsDriver<WindowsElement> desktopDriver;
        private readonly WindowsContext<WindowsDriver<WindowsElement>> appiumContext;

        private AppUISteps(WindowsContext<WindowsDriver<WindowsElement>> appiumContext)
        {
            this.appiumContext = appiumContext;
            desktopDriver = appiumContext.DesktopSession.RemoteDriver;
            browser = appiumContext.AppBrowser;
        }

        [When(@"I save a picture of ""(.*)"" as ""(.*)""")]
        public void WhenISaveAPictureOfAs(INavigable page, string expectedImageName)
        {
            browser.Last
                .GoTo(page)
                .Do(() =>
                {
                    // Ensure to not overlap a control.
                    new Actions(desktopDriver)
                        .MoveToElement(appiumContext.Desktop)
                        .Perform();

                    var screenshot = browser.RemoteDriver.GetScreenshot();
                    using var ms = new MemoryStream(screenshot.AsByteArray);
                    using var bmp = new Bitmap(ms);
                    using var crop = TrimImage(bmp, 5);
                    var pagePath = $"{expectedImageName}.png";
                    crop.Save(pagePath, ImageFormat.Png);
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
            Rectangle boudingRectangle = new Rectangle(
                trimPixelCount,
                trimPixelCount,
                image.Width - trimPixelCount,
                image.Height - trimPixelCount);
            Bitmap retVal = new Bitmap(boudingRectangle.Width, boudingRectangle.Height);
            Graphics graphic = Graphics.FromImage(retVal);
            graphic.DrawImage(image, -boudingRectangle.X / 2, -boudingRectangle.Y / 2);
            return retVal;
        }
    }
}