using FluentAssertions;
using IC.Navigation;
using IC.Tests.App.Poms;
using IC.Tests.App.Poms.Appium;
using IC.Tests.App.Poms.Appium.POMs;
using IC.TimeoutEx;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;

namespace NavBrowser.Tests
{
    public class UnitTest1
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

        public WindowsDriver<WindowsElement> WinDriver => new WindowsDriver<WindowsElement>(
                    Uri,
                    AppiumOptions,
                    TimeSpan.FromSeconds(300));

        private string GetTargetFullPath()
        {
            var splited = Environment.CurrentDirectory.Split('\\').ToList();
            var build = splited.ElementAt(splited.IndexOf("bin") + 1);
            var testsDir = Environment.CurrentDirectory.Replace($@"Browser.Tests\bin\{build}\netcoreapp3.0", "");
            string path = $@"{testsDir}\IC.Tests.App\bin\{build}\IC.Tests.App.exe";
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The application under tests \"{path}\" was not found.");
            }

            return path;
        }

        [Fact]
        public void Test1()
        {
            using var globalCancellationTokenSource = new CancellationTokenSource(10.m());
            var globalCancellationToken = globalCancellationTokenSource.Token;
            var map = new Map<WindowsDriver<WindowsElement>>(WinDriver, globalCancellationToken);
            var nav = new Navigator(map);
            var browser = new Browser(map, nav);
            browser.Navigator.WaitForExist(map.PomMenu, globalCancellationToken);
            browser.Navigator.GoTo(map.PomMenu, map.PomYellow, globalCancellationToken);
            browser.Navigator.GoTo(map.PomYellow, map.PomRed, globalCancellationToken);
            browser.Navigator.GoTo(map.PomRed, map.PomBlue, globalCancellationToken);
            browser.Navigator.GoTo(map.PomBlue, map.PomYellow, globalCancellationToken);
            browser.Navigator.GoTo(map.PomYellow, map.PomRed, globalCancellationToken);
            browser.Navigator.GoTo(map.PomRed, map.PomYellow, globalCancellationToken);
            browser.Navigator.GoTo(map.PomYellow, map.PomYellow, globalCancellationToken);
            browser.Navigator.GoTo(browser.Map.Log.Last, map.PomMenu, globalCancellationToken);
            browser.Navigator.GoTo(map.PomMenu, map.PomYellow, globalCancellationToken);
            browser.Navigator.Do<PomMenu<WindowsDriver<WindowsElement>>>(map.PomYellow, (x) => map.PomYellow.OpenMenuByMenuBtn(TimeSpan.FromSeconds(10)), globalCancellationToken);
            browser.Navigator.Back(globalCancellationToken);
        }
    }
}