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
    public class Map<R> : IMap, INavigables<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        public PomRed<R> PomRed => GetNavigable<PomRed<R>>();
        public PomBlue<R> PomBlue => GetNavigable<PomBlue<R>>();
        public PomMenu<R> PomMenu => GetNavigable<PomMenu<R>>();
        public PomYellow<R> PomYellow => GetNavigable<PomYellow<R>>();

        public readonly R RemoteDriver;

        private readonly CancellationToken globalCancellationToken;

        /// <summary>
        /// The nodes of INavigables forming the Graph.
        /// </summary>
        public HashSet<INavigable> Nodes { get; }

        public HashSet<DynamicNeighbor> DynamicNeighbors { get; private set; }

        public IGraph Graph { get; }

        private readonly ILog log;

        public Map(R remoteDriver, ILog log, CancellationToken globalCancellationToken)
        {
            DynamicNeighbors = new HashSet<DynamicNeighbor>();
            this.log = log;
            RemoteDriver = remoteDriver;
            Nodes = GetNodesByReflection<R>(Assembly.GetExecutingAssembly());
            Graph = new Graph(Nodes);
            this.globalCancellationToken = globalCancellationToken;
            AddDynamicNeighbors();
        }

        #region private

        private void AddDynamicNeighbors()
        {
            foreach (var page in Nodes)
            {
                foreach (var dynamicNeighbor in page.GetDynamicNeighbors())
                {
                    DynamicNeighbors.Add(dynamicNeighbor);
                }
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
                var instance = Activator.CreateInstance(t, this, log, globalCancellationToken) as INavigable;
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