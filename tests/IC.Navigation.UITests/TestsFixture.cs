using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Navigation.UITests
{
    public class TestsFixture : IDisposable
    {
        bool diposed = false;
        public readonly IAppBrowser<WindowsDriver<WindowsElement>> AppBrowser;

        public TestsFixture()
        {
            AppBrowser = new WindowsContext<WindowsDriver<WindowsElement>>().AppBrowser;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (diposed)
            {
                return;
            }

            if (disposing)
            {
                AppBrowser?.Dispose();
            }

            diposed = true;
        }
    }
}