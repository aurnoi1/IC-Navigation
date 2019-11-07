using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using TechTalk.SpecFlow;

namespace IC.Navigation.UITests.Specflow.StepArgumentTransformations
{
    [Binding]
    public class WindowsElementTransformations
    {
        private readonly IBrowser<WindowsDriver<WindowsElement>> sut;

        public WindowsElementTransformations(AppiumContext<WindowsDriver<WindowsElement>> appiumContext)
        {
            this.sut = appiumContext.SUT;
        }

        [StepArgumentTransformation]
        public WindowsElement AliasToWindowsElement(string alias)
        {
            return sut.FindElementByAliasesInLastINavigable(alias);
        }
    }
}