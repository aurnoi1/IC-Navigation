using IC.Navigation.UITests.AppiumConfiguration;
using IC.Navigation.UITests.Interfaces;
using IC.Tests.App.Poms.Appium;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace IC.Navigation.UITests.Specflow.Contexts
{
    public class AppiumContext<R> where R : WindowsDriver<WindowsElement>
    {
        private IAppBrowser<R> sut;

        /// <summary>
        /// An instance of the SUT's IMySession.
        /// </summary>
        public IAppBrowser<R> SUT
        {
            get
            {
                lock (_lock)
                {
                    if (sut == null || sut.RemoteDriver.SessionId == null)
                    {
                        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                        {
                            sut = Start(cts.Token);
                        }
                    }
                }

                return sut;
            }
        }

        private readonly object _lock = new object();

        /// <summary>
        /// Start the AppBrowser and wait for UI to exists.
        /// </summary>
        /// <param name="ct">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The SUT session.</returns>
        public IAppBrowser<R> Start(CancellationToken ct)
        {
            var sut = GetAppBrowser();
            sut.WaitForEntryPoints(ct);
            return sut;
        }

        /// <summary>
        /// Start the AppBrowser without waiting for UI to exists.
        /// </summary>
        /// <returns>The SUT session.</returns>
        public IAppBrowser<R> GetAppBrowser()
        {
            IAppiumSession<R> session = GetAppiumSession();
            var sut = new AppBrowser<R>(session);
            return sut;
        }

        public IAppiumSession<R> GetAppiumSession()
        {
            IAppiumConfig config = new AppiumConfig();
            IAppiumSession<R> session = new AppiumSession<R>(config);
            return session;
        }
    }
}