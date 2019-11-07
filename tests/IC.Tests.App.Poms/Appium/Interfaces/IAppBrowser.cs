using IC.Navigation.Extensions.Appium.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Remote;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface IAppBrowser<R> : INavigables<R>, IRemoteDriverBrowser<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
    }
}