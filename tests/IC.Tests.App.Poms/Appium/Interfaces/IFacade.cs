using IC.Navigation.Extensions.Appium.Interfaces;
using IC.Navigation.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Remote;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface IFacade<R> : INavigatorSession, INavigables<R>, IRemoteDriverSession<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
    }
}