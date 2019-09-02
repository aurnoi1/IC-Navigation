using OpenQA.Selenium.Appium;
using System;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface IAppiumConfig
    {
        Uri Uri { get; }
        AppiumOptions AppiumOptions { get; }
    }
}