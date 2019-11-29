//using IC.Navigation.Interfaces;
//using IC.Tests.App.Poms.Appium;
//using System.Reflection;

//namespace IC.Navigation.CoreExtensions
//{
//    public static class NavigatorSessionEx
//    {
//        /// <summary>
//        /// Get INavigable by their attribute Aliases.
//        /// </summary>
//        /// <param name="alias">The expected alias.</param>
//        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
//        public static INavigable GetINavigableByUsageName(this INavigator navigatorSession, string alias)
//        {
//            INavigable iNavigable = null;
//            foreach (var node in navigatorSession.Graph.Nodes)
//            {
//                var aliases = node.GetType().GetCustomAttribute<Aliases>(true);
//                if (aliases != null && aliases.Values.Contains(alias))
//                {
//                    iNavigable = node;
//                }
//            }

//            return iNavigable;
//        }
//    }
//}