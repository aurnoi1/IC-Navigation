using IC.Navigation.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace IC.Navigation.CoreExtensions
{
    public static class NavigableEx
    {
        /// <summary>
        /// Executes the action passed in parameter.
        /// </summary>
        /// <param name="source">This INavigable instance.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>The current INavigable.</returns>
        public static INavigable Do(this INavigable source, Action action)
        {
            return source.Session.Do(source, action);
        }

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <param name="source">This INavigable instance.</param>
        /// <param name="function">The Function to execute.</param>
        /// <returns>The expected INavigable returns by the Function.</returns>
        public static INavigable Do<T>(this INavigable source, Func<INavigable> function) where T : INavigable
        {
            return source.Session.Do<T>(source, function);
        }

        /// <summary>
        /// Find the shortest path of navigation to go to the destination INavigable,
        /// then performs actions through other INavigables.
        /// </summary>
        /// <param name="source">This INavigable instance.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The destination.</returns>
        public static INavigable GoTo(this INavigable source, INavigable destination)
        {
            return source.Session.GoTo(source, destination);
        }

        /// <summary>
        /// Go to the previous INavigable.
        /// </summary>
        /// <param name="source">This INavigable instance.</param>
        /// <returns>The previous INavigable.</returns>
        public static INavigable Back(this INavigable source)
        {
            return source.Session.Back();
        }

        /// <summary>
        /// Wait for INavigable to exists with the default <see cref="ISession.ThinkTime"/>.
        /// </summary>
        /// <param name="source">This INavigable instance.</param>
        /// <returns><c>true</c> if exists, otherwise <c>false</c>.</returns>
        public static bool WaitForExists(this INavigable source)
        {
            return source.PublishStatus().Exists;
        }

        /// <summary>
        /// Wait for INavigable to exists with a specified <see cref="ISession.ThinkTime"/>.
        /// </summary>
        /// <param name="source">This INavigable instance.</param>
        /// <param name="ephemeralThinkTime">An ephemeral ThinkTime set for this action only.</param>
        /// <returns><c>true</c> if exists, otherwise <c>false</c>.</returns>
        public static bool WaitForExists(this INavigable source, uint ephemeralThinkTime)
        {
            var actualThinkTime = source.Session.ThinkTime;
            try
            {
                source.Session.ThinkTime = ephemeralThinkTime;
                return source.PublishStatus().Exists;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                source.Session.ThinkTime = actualThinkTime;
            }
        }
    }
}