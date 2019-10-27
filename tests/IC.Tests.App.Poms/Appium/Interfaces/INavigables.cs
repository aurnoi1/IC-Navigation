using IC.Tests.App.Poms.Appium.POMs;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Remote;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface INavigables<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        PomBlue<R> PomBlue { get; }
        PomMenu<R> PomMenu { get; }
        PomRed<R> PomRed { get; }
        PomYellow<R> PomYellow { get; }
    }
}