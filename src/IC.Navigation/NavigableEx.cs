using IC.Navigation.Interfaces;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IC.Navigation.CoreExtensions
{
    public static class NavigableEx
    {
        /// <summary>
        /// Executes the action passed in parameter.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The current INavigable.</returns>
        public static INavigable Do(this INavigable source, Action action, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return source.Session.Do(source, action, cancellationToken);
        }

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <param name="function">The Function to execute.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The expected INavigable returns by the Function.</returns>
        public static INavigable Do<T>(
            this INavigable source, 
            Func<INavigable> function, 
            CancellationToken cancellationToken) where T : INavigable
        {
            cancellationToken.ThrowIfCancellationRequested();
            return source.Session.Do<T>(source, function, cancellationToken);
        }

        /// <summary>
        /// Find the shortest path of navigation to go to the destination INavigable,
        /// then performs actions through other INavigables.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The destination.</returns>
        public static INavigable GoTo(this INavigable source, INavigable destination, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return source.Session.GoTo(source, destination, cancellationToken);
        }

        /// <summary>
        /// Go to the previous INavigable.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The previous INavigable.</returns>
        public static INavigable Back(this INavigable source, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return source.Session.Back(cancellationToken);
        }

        /// <summary>
        /// Get the <see cref="INavigableStatus.Exists"/> status of the Navigable./>.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <returns><c>true</c> if exists, otherwise <c>false</c>.</returns>
        public static bool Exists(this INavigable source)
        {
            return source.PublishStatus().Exists;
        }
    }
}