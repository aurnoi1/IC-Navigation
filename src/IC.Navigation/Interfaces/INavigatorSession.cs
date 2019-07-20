using System;
using System.Collections.Generic;
using System.Reflection;

namespace IC.Navigation.Interfaces
{
    public interface INavigatorSession : ISession
    {
        /// <summary>
        /// Get the instance of INavigable from the Nodes.
        /// </summary>
        /// <typeparam name="T">The returned instance type.</typeparam>
        /// <param name="type">The type requested.</param>
        /// <returns>The instance of the requested INavigable.</returns>
        T GetINavigableInstance<T>(Type type) where T : INavigable;

        /// <summary>
        /// Get the nodes formed by instances of INavigables from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing the INavigables.</param>
        /// <returns>Intances of INavigables forming the nodes.</returns>
        HashSet<INavigable> GetNodesByReflection(Assembly assembly);
    }
}