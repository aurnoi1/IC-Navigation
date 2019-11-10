using ApprovalTests;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using TechTalk.SpecFlow;

namespace IC.Navigation.UITests.Specflow.Steps
{
    [Binding]
    public class UITestHook
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Approvals.UseAssemblyLocationForApprovedFiles();
        }
    }
}