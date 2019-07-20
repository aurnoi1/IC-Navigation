using IC.Navigation.UITests.Configs;
using IC.Navigation.UITests.Interfaces;
using IC.Tests.App.UIAccessibility.Appium;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;

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
            ISUTAppiumConfig config = new SUTAppiumConfig();
            IAppiumSession session = new SUTAppiumSession(config);
            var sut = new Facade(session);
            sut.WaitForEntryPoints();
            return sut;
        }
    }
}