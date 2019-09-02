using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IC.Navigation.CoreExtensions
{
    public static class SessionEx
    {
        /// <summary>
        /// Get INavigable by their attribute Aliases.
        /// </summary>
        /// <param name="alias">The expected alias.</param>
        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
        public static INavigable GetINavigableByUsageName(this ISession navigatorSession, string alias)
        {
            INavigable iNavigable = null;
            foreach (var node in navigatorSession.Graph.Nodes)
            {
                var aliases = node.GetType().GetCustomAttribute<Aliases>(true);
                if (aliases != null && aliases.Values.Contains(alias))
                {
                    iNavigable = node;
                }
            }

            return iNavigable;
        }
    }
}
