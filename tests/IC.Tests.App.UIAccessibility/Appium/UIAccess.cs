using IC.Navigation;
using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewNavigables;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IC.Tests.App.UIAccessibility.Appium
{
    public class UIAccess : NavigatorSession, IUIAccess
    {
        public UIAccess(IAppiumSession appiumSession)
        {
            ThinkTime = 1;
            WindowsDriver = appiumSession.WindowsDriver;
            EntryPoints = new HashSet<INavigable>() { ViewMenu };
            Graph = new Graph(GetNodesByReflection(Assembly.GetExecutingAssembly()));
        }

        public UIAccess(IAppiumSession appiumSession, uint thinkTime) : this(appiumSession)
        {
            ThinkTime = thinkTime;
        }

        public UIAccess(WindowsDriver<WindowsElement> winDriver, HashSet<INavigable> entryPoints, uint thinkTime)
        {
            ThinkTime = thinkTime;
            WindowsDriver = winDriver;
            Graph = new Graph(GetNodesByReflection(Assembly.GetExecutingAssembly()));
            EntryPoints = entryPoints;
        }

        #region Properties

        #region Private

        private bool disposed = false;

        #endregion Private

        #region Public

        #region Views

        public IViewRed ViewRed => new ViewRed(this);
        public IViewBlue ViewBlue => new ViewBlue(this);
        public IViewMenu ViewMenu => new ViewMenu(this);
        public IViewYellow ViewYellow => new ViewYellow(this);

        #endregion Views

        /// <summary>
        /// The WindowsDriver used to connect to the application.
        /// </summary>
        public WindowsDriver<WindowsElement> WindowsDriver { get; private set; }

        /// <summary>
        /// Get the Graph containing the INavigables.
        /// </summary>
        public override IGraph Graph { get; }

        /// <summary>
        /// The INavigables to be expected as entry points when the application start.
        /// </summary>
        public override HashSet<INavigable> EntryPoints { get; protected set; }

        /// <summary>
        /// Multiplicator to adjust the timeouts when waiting for UI objects.
        /// </summary>
        public override uint ThinkTime { get; set; }

        #endregion Public

        #endregion Properties

        #region Methods

        #region Public

        /// <summary>
        /// Dispose this Instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public

        #region Private

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                WindowsDriver?.Dispose();
            }

            disposed = true;
        }

        #endregion Private

        #endregion Methods
    }
}