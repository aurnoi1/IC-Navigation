using OpenQA.Selenium.Appium.Windows;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface IAppiumSession
    {
        WindowsDriver<WindowsElement> WindowsDriver { get; }
    }
}