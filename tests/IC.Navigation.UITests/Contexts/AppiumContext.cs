using IC.Navigation.UITests.Configs;
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
        private IBrowser<R> sut;

        /// <summary>
        /// An instance of the SUT's IMySession.
        /// </summary>
        public IBrowser<R> SUT
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
        /// Start the Appium session and wait for UI to exists.
        /// </summary>
        /// <param name="ct">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The SUT session.</returns>
        public IBrowser<R> Start(CancellationToken ct)
        {
            var sut = GetFacade();
            sut.WaitForEntryPoints(ct);
            return sut;
        }

        /// <summary>
        /// Start the Appium session without waiting for UI to exists.
        /// </summary>
        /// <returns>The SUT session.</returns>
        public IBrowser<R> GetFacade()
        {
            IShallowRemoteDriver<R> session = GetAppiumSession();
            var sut = new Browser<R>(session);
            return sut;
        }

        public IShallowRemoteDriver<R> GetAppiumSession()
        {
            ISUTAppiumConfig config = new SUTAppiumConfig();
            IShallowRemoteDriver<R> session = new SUTAppiumSession<R>(config);
            return session;
        }
    }
}