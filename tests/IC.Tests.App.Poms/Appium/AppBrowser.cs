using IC.Navigation;
using IC.Navigation.Exceptions;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using IC.Tests.App.Poms.Appium.POMs;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IC.Tests.App.Poms.Appium
{
    public class AppBrowser<R> : Navigator, IAppBrowser<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
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

        #endregion Public

        #endregion Properties

        #region Methods

        #region Public

        /// <summary>
        /// Get the instance of INavigable from the Nodes.
        /// </summary>
        /// <typeparam name="T">The returned instance type.</typeparam>
        /// <returns>The instance of the requested INavigable.</returns>
        public T GetNavigable<T>() where T : INavigable
        {
            Type type = typeof(T);
            var match = Nodes.Where(n => n.GetType() == type).SingleOrDefault();
            if (match != null)
            {
                return (T)match;
            }
            else
            {
                throw new UnregistredNodeException(type);
            }
        }

        /// <summary>
        /// Get the nodes formed by instances of INavigables from the specified assembly.
        /// </summary>
        /// <typeparam name="T">The generic type of the classes implementing INavigable.</typeparam>
        /// <param name="assembly">The assembly containing the INavigables.</param>
        /// <returns>Intances of INavigables forming the nodes.</returns>
        public HashSet<INavigable> GetNodesByReflection<T>(Assembly assembly)
        {
            var navigables = new HashSet<INavigable>();
            var iNavigables = GetINavigableTypes(assembly);
            foreach (var iNavigable in iNavigables)
            {
                var t = iNavigable.MakeGenericType(typeof(T));
                var instance = Activator.CreateInstance(t, this) as INavigable;
                navigables.Add(instance);
            }

            return navigables;
        }

        /// <summary>
        /// Update the observer with this Navigable's State.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="state">The State.</param>
        public override void Update<T>(INavigable navigable, IState<T> state)
        {
            // Add a logger here if wanted.
            throw new NotImplementedException();
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

        private List<Type> GetINavigableTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(x =>
                    typeof(INavigable).IsAssignableFrom(x)
                    && !x.IsInterface
                    && x.IsPublic
                    && !x.IsAbstract
                ).ToList();
        }

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