using IC.Navigation;
using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewNavigables;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IC.Tests.App.UIAccessibility.Appium
{
    public class MySession : Navigator, IMySession
    {
        #region Fields

        private HashSet<INavigable> navigables;

        #endregion Fields

        public MySession(IAppiumSession appiumSession)
        {
            ThinkTime = 1;
            WindowsDriver = appiumSession.WindowsDriver;
            EntryPoints = new HashSet<INavigable>() { ViewMenu };
            Graph = new Graph(Nodes);
        }

        public MySession(IAppiumSession appiumSession, uint thinkTime) : this(appiumSession)
        {
            ThinkTime = thinkTime;
        }

        public MySession(WindowsDriver<WindowsElement> winDriver, HashSet<INavigable> entryPoints, uint thinkTime)
        {
            ThinkTime = thinkTime;
            WindowsDriver = winDriver;
            Graph = new Graph(Nodes);
            EntryPoints = entryPoints;
        }

        #region Properties

        #region Private

        private bool disposed = false;

        /// <summary>
        /// All the INavigable nodes contains in the Graph of this Navigable.
        /// </summary>
        private HashSet<INavigable> Nodes
        {
            get
            {
                if (navigables == null)
                {
                    navigables = new HashSet<INavigable>();
                    var iNavigables = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(x => typeof(INavigable).IsAssignableFrom(x) && !x.IsInterface)
                        .ToList();

                    foreach (var iNavigable in iNavigables)
                    {
                        var instance = Activator.CreateInstance(iNavigable, this) as INavigable;
                        navigables.Add(instance);
                    }
                }

                return navigables;
            }

            set
            {
                navigables = value;
            }
        }

        #endregion Private

        #region Public

        public override IGraph Graph
        {
            get => base.Graph;
            protected set => base.Graph = value;
        }

        /// <summary>
        /// Multiplicator to adjust the timeout to the environment when waiting for the controls.
        /// </summary>
        public uint ThinkTime { get; set; }

        /// <summary>
        /// Adjust the timeout to the environment when waiting for the controls depending the <see cref="ThinkTime"/> value.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The adjusted timeout.</returns>
        public TimeSpan AdjustTimeout(TimeSpan timeout)
        {
            var adjTimeout = TimeSpan.FromTicks(timeout.Ticks * ThinkTime);
            return adjTimeout;
        }

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
        /// The INavigables to be expected as entry points when the application start.
        /// </summary>
        public HashSet<INavigable> EntryPoints { get; private set; }

        /// <summary>
        /// The INavigable EntryPoint that is found at the beginning of the navigation.
        /// Otherwise <c>null</c> if nothing found at the time.
        /// </summary>
        public INavigable EntryPoint => Historic.FirstOrDefault();

        #endregion Public

        #endregion Properties

        #region Methods

        #region Public

        /// <summary>
        /// Wait for any EntryPoints to exists.
        /// The amount of time to wait is defined by each INavigable.WaitForExists().
        /// </summary>
        /// <returns>The first INavigable found, otherwise <c>null</c>.</returns>
        public INavigable WaitForEntryPoints()
        {
            INavigable entryPoint = null;
            Parallel.ForEach(EntryPoints, (iNavigable, state) =>
            {
                if (!state.IsStopped && iNavigable.WaitForExists())
                {
                    entryPoint = iNavigable;
                    state.Stop();
                }
            });

            return entryPoint;
        }

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
                //WindowsDriver?.Close();
                WindowsDriver?.Dispose();
            }

            disposed = true;
        }

        #endregion Private

        #endregion Methods
    }
}