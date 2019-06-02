using IC.Navigation.UITests.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Navigation.UITests.Configs
{
    public class SUTAppiumSession : IAppiumSession
    {
        private IAppiumConfig appiumConfig;
        private WindowsDriver<WindowsElement> winDriver;

        public SUTAppiumSession(ISUTAppiumConfig sutAppiumtConfig)
        {
            this.appiumConfig = sutAppiumtConfig;
        }

        public WindowsDriver<WindowsElement> WindowsDriver
        {
            get
            {
                if (winDriver == null)
                {
                    WindowsDriver<WindowsElement> windowsDriver = new WindowsDriver<WindowsElement>(
                        appiumConfig.Uri,
                        appiumConfig.AppiumOptions,
                        TimeSpan.FromSeconds(300));

                    winDriver = windowsDriver;
                }

                return winDriver;
            }
        }
    }
}