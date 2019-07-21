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
    public class Facade : NavigatorSession, IFacade
    {
        public Facade(IAppiumSession appiumSession)
        {
            Nodes = GetNodesByReflection(Assembly.GetExecutingAssembly());
            Graph = new Graph(Nodes);
            WindowsDriver = appiumSession.WindowsDriver;
            EntryPoints = new HashSet<INavigable>() { ViewMenu };
        }

        public Facade(IAppiumSession appiumSession, double thinkTime) : this(appiumSession)
        {
            ThinkTime = thinkTime;
        }

        public Facade(WindowsDriver<WindowsElement> winDriver, HashSet<INavigable> entryPoints, double thinkTime)
        {
            ThinkTime = thinkTime;
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

        public ViewRed ViewRed => GetNavigable<ViewRed>();
        public ViewBlue ViewBlue => GetNavigable<ViewBlue>();
        public ViewMenu ViewMenu => GetNavigable<ViewMenu>();
        public ViewYellow ViewYellow => GetNavigable<ViewYellow>();

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

        private double thinkTime = 1;

        /// <summary>
        /// Positive multiplier to adjust the timeouts when waiting for UI objects.
        /// The default value is 1.
        /// </summary>
        public override double ThinkTime
        {
            get
            {
                return thinkTime;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception($"The value of {nameof(ThinkTime)} must be positive.");
                }

                thinkTime = value;
            }
        }

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