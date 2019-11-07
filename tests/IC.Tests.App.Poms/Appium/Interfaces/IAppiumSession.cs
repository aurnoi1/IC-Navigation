using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Remote;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface IAppiumSession<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        R RemoteDriver { get; }
    }
}