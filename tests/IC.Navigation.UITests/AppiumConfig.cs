using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.IO;

namespace IC.Navigation.UITests
{
    internal class AppiumConfig
    {
        private WindowsDriver<WindowsElement> winDriver;

        internal AppiumConfig()
        {
        }

        public WindowsDriver<WindowsElement> WinDriver
        {
            get
            {
                if (winDriver == null)
                {
                    Uri uri = new Uri("http://localhost:4723/wd/hub");
                    string appFullPath = Path.Combine(Environment.CurrentDirectory, "IC.Tests.App.exe");
                    AppiumOptions appiumOptions = new AppiumOptions();
                    appiumOptions.AddAdditionalCapability("app", appFullPath);
                    appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                    WindowsDriver<WindowsElement> windowsDriver = new WindowsDriver<WindowsElement>(uri, appiumOptions, TimeSpan.FromSeconds(300));
                    winDriver = windowsDriver;
                }

                return winDriver;
            }
        }
    }
}