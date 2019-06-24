using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IC.Navigation
{
    /// <summary>
    /// An abstract implementation of INavigator and ISession.
    /// </summary>
    public abstract class NavigatorSession : Navigator, ISession
    {
        /// <summary>
        /// Get the nodes formed by instances of INavigables from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing the INavigables.</param>
        /// <returns>Intances of INavigables forming the nodes.</returns>
        public virtual HashSet<INavigable> GetNodesByReflection(Assembly assembly)
        {
            var navigables = new HashSet<INavigable>();
            var iNavigables = assembly.GetTypes()
                .Where(x => typeof(INavigable).IsAssignableFrom(x) && !x.IsInterface)
                .ToList();

            foreach (var iNavigable in iNavigables)
            {
                var instance = Activator.CreateInstance(iNavigable, this) as INavigable;
                navigables.Add(instance);
            }

            return navigables;
        }

        /// <summary>
        /// The INavigables to be expected as entry points when the application start.
        /// </summary>
        public virtual HashSet<INavigable> EntryPoints { get; protected set; }

        /// <summary>
        /// The INavigable EntryPoint that is found at the beginning of the navigation.
        /// Otherwise <c>null</c> if nothing found at the time.
        /// </summary>
        public virtual INavigable EntryPoint => Historic.FirstOrDefault();

        /// <summary>
        /// Multiplicator to adjust the timeout to the environment when waiting for the controls.
        /// </summary>
        public virtual uint ThinkTime { get; set; }

        /// <summary>
        /// Adjust the timeout to the environment when waiting for the controls depending the <see cref="ThinkTime"/> value.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The adjusted timeout.</returns>
        public virtual TimeSpan AdjustTimeout(TimeSpan timeout)
        {
            var adjTimeout = TimeSpan.FromTicks(timeout.Ticks * ThinkTime);
            return adjTimeout;
        }

        /// <summary>
        /// Wait for any EntryPoints to exists.
        /// The amount of time to wait is defined by each INavigable.WaitForExists().
        /// </summary>
        /// <returns>The first INavigable found, otherwise <c>null</c>.</returns>
        public virtual INavigable WaitForEntryPoints()
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
    }
}