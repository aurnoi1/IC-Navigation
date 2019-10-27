using OpenQA.Selenium.Appium.Windows;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface IAppiumSession<R> where R : WindowsDriver<WindowsElement>
    {
        R WindowsDriver { get; }
    }
}