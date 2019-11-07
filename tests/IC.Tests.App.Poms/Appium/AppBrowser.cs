using IC.Navigation;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using IC.Tests.App.Poms.Appium.POMs;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace IC.Tests.App.Poms.Appium
{
    public class AppBrowser<R> : NavigatorSession, IAppBrowser<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Facade"/> class.
        /// </summary>
        /// <param name="appiumSession">The AppiumSession.</param>
        public AppBrowser(IAppiumSession<R> appiumSession)
        {
            Nodes = GetNodesByReflection<R>(Assembly.GetExecutingAssembly());
            Graph = new Graph(Nodes);
            RemoteDriver = appiumSession.RemoteDriver;
            EntryPoints = new HashSet<INavigable>() { PomMenu };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Facade"/> class.
        /// </summary>
        /// <param name="appiumSession">The AppiumSession.</param>
        /// <param name="globalCancellationToken">The GlobalCancellationToken.</param>
        public AppBrowser(IAppiumSession<R> appiumSession, CancellationToken globalCancellationToken)
        {
            Nodes = GetNodesByReflection<R>(Assembly.GetExecutingAssembly());
            Graph = new Graph(Nodes);
            RemoteDriver = appiumSession.RemoteDriver;
            EntryPoints = new HashSet<INavigable>() { PomMenu };
            GlobalCancellationToken = globalCancellationToken;
        }

        #region Properties

        #region Private

        private bool disposed = false;

        #endregion Private

        #region Public

        #region POMs

        public PomRed<R> PomRed => GetNavigable<PomRed<R>>();
        public PomBlue<R> PomBlue => GetNavigable<PomBlue<R>>();
        public PomMenu<R> PomMenu => GetNavigable<PomMenu<R>>();
        public PomYellow<R> PomYellow => GetNavigable<PomYellow<R>>();

        #endregion POMs

        /// <summary>
        /// The nodes of INavigables forming the Graph.
        /// </summary>
        public override HashSet<INavigable> Nodes { get; }

        /// <summary>
        /// The WindowsDriver used to connect to the application.
        /// </summary>
        public R RemoteDriver { get; private set; }

        /// <summary>
        /// The Cancellation Token used to cancel all the running navigation tasks.
        /// </summary>
        public override CancellationToken GlobalCancellationToken { get; set; }

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
                using (var remoteDriver = RemoteDriver as IDisposable)
                {
                    remoteDriver?.Dispose();
                }
            }

            disposed = true;
        }

        #endregion Private

        #endregion Methods
    }
}