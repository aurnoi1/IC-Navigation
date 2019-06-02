using OpenQA.Selenium.Appium.Windows;

namespace IC.Tests.App.UIAccessibility.Appium.Interfaces
{
    public interface IAppiumSession
    {
        WindowsDriver<WindowsElement> WindowsDriver { get; }
    }
}