using IC.Navigation.UITests.Configs;
using IC.Navigation.UITests.Interfaces;
using IC.Tests.App.Poms.Appium;
using IC.Tests.App.Poms.Appium.Interfaces;
using System;
using System.Threading;

namespace IC.Navigation.UITests.Specflow.Contexts
{
    public class AppiumContext
    {
        private IFacade sut;

        /// <summary>
        /// An instance of the SUT's IMySession.
        /// </summary>
        public IFacade SUT
        {
            get
            {
                lock (_lock)
                {
                    if (sut == null || sut.WindowsDriver.SessionId == null)
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
        public IFacade Start(CancellationToken ct)
        {
            var sut = GetFacade();
            sut.WaitForEntryPoints(ct);
            return sut;
        }

        /// <summary>
        /// Start the Appium session without waiting for UI to exists.
        /// </summary>
        /// <returns>The SUT session.</returns>
        public IFacade GetFacade()
        {
            ISUTAppiumConfig config = new SUTAppiumConfig();
            IAppiumSession session = new SUTAppiumSession(config);
            var sut = new Facade(session);
            return sut;
        }
    }
}