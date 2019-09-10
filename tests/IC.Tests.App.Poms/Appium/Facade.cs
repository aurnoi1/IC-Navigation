using IC.Navigation;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using IC.Tests.App.Poms.Appium.POMs;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IC.Tests.App.Poms.Appium
{
    public class Facade : NavigatorSession, IFacade
    {
        public Facade(IAppiumSession appiumSession)
        {
            Nodes = GetNodesByReflection(Assembly.GetExecutingAssembly());
            Graph = new Graph(Nodes);
            WindowsDriver = appiumSession.WindowsDriver;
            EntryPoints = new HashSet<INavigable>() { PomMenu };
        }

        public Facade(WindowsDriver<WindowsElement> winDriver, HashSet<INavigable> entryPoints, double thinkTime)
        {
            Nodes = GetNodesByReflection(Assembly.GetExecutingAssembly());
            Graph = new Graph(Nodes);
            WindowsDriver = winDriver;
            EntryPoints = entryPoints;
        }

        #region Properties

        #region Private

        private bool disposed = false;

        #endregion Private

        #region Public

        #region Views

        public PomRed PomRed => GetNavigable<PomRed>();
        public PomBlue PomBlue => GetNavigable<PomBlue>();
        public PomMenu PomMenu => GetNavigable<PomMenu>();
        public PomYellow PomYellow => GetNavigable<PomYellow>();

        #endregion Views

        /// <summary>
        /// The nodes of INavigables forming the Graph.
        /// </summary>
        public override HashSet<INavigable> Nodes { get; }

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