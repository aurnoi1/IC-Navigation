using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface IWindowsDriverSession<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        R WindowsDriver { get; }
    }
}