using IC.Navigation.UITests.AppiumConfiguration;
using IC.Navigation.UITests.Interfaces;
using IC.Tests.App.Poms.Appium;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace IC.Navigation.UITests.Specflow.Contexts
{
    public class WindowsContext<R> where R : WindowsDriver<WindowsElement>
    {
        private IAppBrowser<R> appBrowser;
        private IAppiumSession<R> desktopSession;

        /// <summary>
        /// An instance of the SUT's IMySession.
        /// </summary>
        public IAppBrowser<R> AppBrowser
        {
            get
            {
                lock (_appLock)
                {
                    if (appBrowser == null || appBrowser.RemoteDriver.SessionId == null)
                    {
                        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                        {
                            appBrowser = Start(cts.Token);
                        }
                    }
                }

                return appBrowser;
            }
        }

        private readonly object _appLock = new object();
        private readonly object _desktopLock = new object();

        public IAppiumSession<R> DesktopSession
        {
            get
            {
                lock (_desktopLock)
                {
                    if (desktopSession == null || desktopSession.RemoteDriver.SessionId == null)
                    {
                        desktopSession = CreateDesktopAppiumSession();
                    }
                }

                return desktopSession;
            }
        }

        public WindowsElement Desktop
        {
            get
            {
                return DesktopSession.RemoteDriver.FindElementByName("Desktop 1");
            }
        }

        /// <summary>
        /// Start the AppBrowser and wait for UI to exists.
        /// </summary>
        /// <param name="ct">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The SUT session.</returns>
        public IAppBrowser<R> Start(CancellationToken ct)
        {
            var sut = CreateAppBrowser();
            sut.WaitForEntryPoints(ct);
            return sut;
        }

        /// <summary>
        /// Start the AppBrowser without waiting for UI to exists.
        /// </summary>
        /// <returns>The SUT session.</returns>
        public IAppBrowser<R> CreateAppBrowser()
        {
            IAppiumSession<R> session = CreateAppAppiumSession();
            var sut = new AppBrowser<R>(session);
            return sut;
        }

        public IAppiumSession<R> CreateAppAppiumSession()
        {
            IAppiumConfig config = new AppAppiumConfig();
            IAppiumSession<R> session = new AppiumSession<R>(config);
            return session;
        }

        public IAppiumSession<R> CreateDesktopAppiumSession()
        {
            IAppiumConfig config = new DesktopAppiumConfig();
            IAppiumSession<R> session = new AppiumSession<R>(config);
            return session;
        }
    }
}