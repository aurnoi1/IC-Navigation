using IC.Navigation.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace IC.Navigation.Chain
{
    public static class INavigableEx
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
        /// Performs action to step to the next INavigable.
        /// The next INavigable must be consecutive to the current INavigable.
        /// </summary>
        /// <param name="destination">The opened INavigable.</param>
        /// <param name="source">This INavigable instance.</param>
        /// <param name="destination">The opened INavigable.</param>
        public static INavigable StepToNext(this INavigable source, INavigable destination)
        {
            var navigableAndAction = source.GetActionToNext().Where(x => x.Key.CompareTypeName(destination)).SingleOrDefault();
            INavigable nextNavigableRef = navigableAndAction.Key;
            Action actionToOpen = navigableAndAction.Value;
            if (nextNavigableRef == null)
            {
                throw new ArgumentException($"The INavigable \"{destination}\" is not available in \"{MethodBase.GetCurrentMethod().DeclaringType}\".");
            }

            actionToOpen.Invoke();
            destination.WaitForExists();
            if (!destination.Session.Last.CompareTypeName(destination))
            {
                throw new Exception($"{destination.ToString()} is not opened.");
            }

            return destination.Session.Last;
        }

        /// <summary>
        /// Compares the Type name of this INavigable to another one.
        /// </summary>
        /// <param name="source">This INavigable instance.</param>
        /// <param name="other">The other INavigable.</param>
        /// <returns><c>true</c> if same, otherwise <c>false</c>.</returns>
        public static bool CompareTypeName(this INavigable source, INavigable other)
        {
            return source.Session.CompareTypeNames(source, other);
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
    }
}