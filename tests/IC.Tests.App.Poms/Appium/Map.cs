using IC.Navigation;
using IC.Navigation.Enums;
using IC.Navigation.Exceptions;
using IC.Navigation.Interfaces;
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
    public class Map<R> : IMap where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        public PomRed<R> PomRed => GetNavigable<PomRed<R>>();
        public PomBlue<R> PomBlue => GetNavigable<PomBlue<R>>();
        public PomMenu<R> PomMenu => GetNavigable<PomMenu<R>>();
        public PomYellow<R> PomYellow => GetNavigable<PomYellow<R>>();

        public R RemoteDriver { get; private set; }

        public CancellationToken GlobalCancellationToken { get; set; }

        /// <summary>
        /// The nodes of INavigables forming the Graph.
        /// </summary>
        public HashSet<INavigable> Nodes { get; }
        public IGraph Graph { get; }

        public Map(R remoteDriver, CancellationToken globalCancellationToken)
        {
            RemoteDriver = remoteDriver;
            Nodes = GetNodesByReflection<R>(Assembly.GetExecutingAssembly());
            Graph = new Graph(Nodes);
            GlobalCancellationToken = globalCancellationToken;
        }

        public void Resolve(INavigable source, IOnActionAlternatives onActionAlternatives, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lock to queue historic entries.
        /// </summary>
        private readonly object historicLock = new object();

        public virtual INavigable Previous
        {
            get
            {
                lock (historicLock)
                {
                    return Historic.Count > 1 ? Historic[Historic.Count - 2] : null;
                }
            }
        }

        public INavigable Last
        {
            get
            {
                lock (historicLock)
                {
                    return Historic.LastOrDefault();
                }
            }

            private set
            {
                lock (historicLock)
                {
                    if (value != null && value != Historic.LastOrDefault())
                    {
                        Historic.Add(value);
                    }
                }
            }
        }

        public List<INavigable> Historic { get; private set; } = new List<INavigable>();

        public void Update(INavigableStatus status)
        {
            SetLast(status.Exist);
        }

        public void Update<T>(IState<T> state)
        {
            if (state.Name == StatesNames.Exist)
            {
                Last = state.Navigable;
            }
        }


        #region private

        /// <summary>
        /// Set the last known INavigable is exists.
        /// </summary>
        /// <param name="status">The NavigableStatus of the last INavigable.</param>
        /// <returns><c>true</c> if the INavigable exists, otherwise <c>false</c>.</returns>
        private void SetLast(IState<bool> state)
        {
            if (state.Name == StatesNames.Exist)
            {
                Last = state.Navigable;
            }
        }

        /// <summary>
        /// Get the instance of INavigable from the Nodes.
        /// </summary>
        /// <typeparam name="T">The returned instance type.</typeparam>
        /// <returns>The instance of the requested INavigable.</returns>
        private T GetNavigable<T>() where T : INavigable
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
        private HashSet<INavigable> GetNodesByReflection<T>(Assembly assembly)
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


        #endregion private
    }
}