using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IC.Navigation.Interfaces
{
    public static class NavigatorSessionEx
    {

        /// <summary>
        /// Get INavigable by their attribute UIArtifact.UsageName.
        /// </summary>
        /// <param name="usageName">The expected usage name.</param>
        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
        public static INavigable GetINavigableByUsageName(this INavigatorSession navigatorSession, string usageName)
        {
            INavigable iNavigable = null;
            foreach (var node in navigatorSession.Graph.Nodes)
            {
                var uIArtefact = node.GetType().GetCustomAttribute<UIArtifact>(true);
                if (uIArtefact != null && usageName == uIArtefact.UsageName)
                {
                    iNavigable = node;
                }
            }

            return iNavigable;
        }
    }
}
