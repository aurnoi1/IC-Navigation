using IC.Navigation.UITests.Interfaces;
using OpenQA.Selenium.Appium;
using System;
using System.IO;
using System.Linq;

namespace IC.Navigation.UITests.AppiumConfiguration
{
    public class DesktopAppiumConfig : IAppiumConfig
    {
        public Uri Uri { get => new Uri("http://localhost:4723/wd/hub"); }

        public AppiumOptions AppiumOptions
        {
            get
            {
                AppiumOptions appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", "Root");
                appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                return appiumOptions;
            }
        }
    }
}