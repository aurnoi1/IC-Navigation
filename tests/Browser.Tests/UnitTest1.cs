using FluentAssertions;
using IC.Tests.App.Poms;
using IC.Tests.App.Poms.Appium;
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
            using var globalCancellationTokenSource = new CancellationTokenSource(10.s());
            var globalCancellationToken = globalCancellationTokenSource.Token;
            var map = new Map<WindowsDriver<WindowsElement>>(WinDriver, globalCancellationToken);

            var nav = new Nav(map);
            nav.Nodes.Should().BeEquivalentTo(map.Nodes);
            nav.Graph.Should().NotBeNull();
            nav.Graph.Nodes.Should().BeEquivalentTo(map.Nodes);
            var browser = new Browser(map, nav);
            browser.Navigator.WaitForExist(map.PomMenu, globalCancellationToken);
            browser.Navigator.GoTo(map.PomMenu, map.PomRed, globalCancellationToken);
        }
    }
}