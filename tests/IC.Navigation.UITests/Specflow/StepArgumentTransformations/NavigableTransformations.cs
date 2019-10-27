using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using TechTalk.SpecFlow;

namespace IC.Navigation.UITests.Specflow.StepArgumentTransformations
{
    [Binding]
    public class NavigableTransformations
    {
        private readonly IFacade<WindowsDriver<WindowsElement>> sut;

        public NavigableTransformations(AppiumContext<WindowsDriver<WindowsElement>> appiumContext)
        {
            this.sut = appiumContext.SUT;
        }

        [StepArgumentTransformation]
        public INavigable AliasToWindowsElement(string alias)
        {
            return sut.GetINavigableByUsageName(alias);
        }
    }
}