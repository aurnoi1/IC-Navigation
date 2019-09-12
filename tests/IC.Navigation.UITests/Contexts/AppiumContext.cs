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
                        sut = Create();
                    }
                }

                return sut;
            }
        }

        private readonly object _lock = new object();

        private IFacade Create()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                ISUTAppiumConfig config = new SUTAppiumConfig();
                IAppiumSession session = new SUTAppiumSession(config);
                var sut = new Facade(session);
                sut.WaitForEntryPoints(cts.Token);
                return sut;
            }
        }
    }
}