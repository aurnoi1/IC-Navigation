using IC.Navigation.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Remote;
using System;

namespace IC.Navigation.Extensions.Appium.Interfaces
{
    public interface IRemoteDriverSession<R> : ISession, IDisposable where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        /// <summary>
        /// The RemoteDriver.
        /// </summary>
        R RemoteDriver { get; }
    }
}