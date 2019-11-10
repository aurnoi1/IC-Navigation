using IC.Navigation.UITests.Interfaces;
using OpenQA.Selenium.Appium;
using System;
using System.IO;
using System.Linq;

namespace IC.Navigation.UITests.AppiumConfiguration
{
    public class AppAppiumConfig : IAppiumConfig
    {
        public Uri Uri { get => new Uri("http://localhost:4723/wd/hub"); }

        public AppiumOptions AppiumOptions
        {
            get
            {
                string appFullPath = GetTargetFullPath();
                AppiumOptions appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", appFullPath);
                appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                return appiumOptions;
            }
        }

        private string GetTargetFullPath()
        {
            var splited = Environment.CurrentDirectory.Split('\\').ToList();
            var build = splited.ElementAt(splited.IndexOf("bin") + 1);
            var testsDir = Environment.CurrentDirectory.Replace($@"IC.Navigation.UITests\bin\{build}\netcoreapp3.0", "");
            string path = $@"{testsDir}IC.Tests.App\bin\{build}\IC.Tests.App.exe";
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The application under tests \"{path}\" was not found.");
            }

            return path;
        }
    }
}