using IC.Navigation.UITests.Configs;
using IC.Navigation.UITests.Interfaces;
using IC.Tests.App.UIAccessibility.Appium;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;

namespace IC.Navigation.UITests.Specflow.Contexts
{
    internal class AppiumContext
    {
        private IUIAccess sut;

        /// <summary>
        /// An instance of the SUT's IMySession.
        /// </summary>
        internal IUIAccess SUT
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

        private object _lock = new object();

        private IUIAccess Create()
        {
            ISUTAppiumConfig config = new SUTAppiumConfig();
            IAppiumSession session = new SUTAppiumSession(config);
            var sut = new UIAccess(session);
            sut.WaitForEntryPoints();
            return sut;
        }
    }
}