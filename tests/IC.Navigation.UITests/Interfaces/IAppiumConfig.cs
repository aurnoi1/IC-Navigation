using OpenQA.Selenium.Appium;
using System;

namespace IC.Navigation.UITests.Interfaces
{
    public interface IAppiumConfig
    {
        Uri Uri { get; }
        AppiumOptions AppiumOptions { get; }
    }
}