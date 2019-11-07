using IC.Navigation.UITests.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Navigation.UITests.Configs
{
    public class SUTAppiumSession<R> : IShallowRemoteDriver<R> where R : WindowsDriver<WindowsElement>
    {
        private IAppiumConfig appiumConfig;
        private WindowsDriver<WindowsElement> winDriver;

        public SUTAppiumSession(ISUTAppiumConfig sutAppiumtConfig)
        {
            this.appiumConfig = sutAppiumtConfig;
        }

        public R RemoteDriver
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

                return winDriver as R;
            }
        }
    }
}