using IC.Navigation.Interfaces;
using System;
using System.Threading;

namespace IC.Navigation.CoreExtensions
{
    public static class NavigableEx
    {
        /// <summary>
        /// Executes the action passed in parameter.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <param name="action">The non-cancellable Action to execute.</param>
        /// <returns>The current INavigable.</returns>
        public static INavigable Do(this INavigable source, Action action)
        {
            using var infinitTokenSource = new CancellationTokenSource();
            Action<CancellationToken> actionNotCancellable = (ct) => action();
            return source.Session.Do(source, actionNotCancellable, infinitTokenSource.Token);
        }

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <param name="function">The non-cancellable Function to execute.</param>
        /// <returns>The expected INavigable returns by the Function.</returns>
        public static INavigable Do<T>(
            this INavigable source,
            Func<INavigable> function) where T : INavigable
        {
            using var infinitTokenSource = new CancellationTokenSource();
            Func<CancellationToken, INavigable> functionCancellable = (ct) => function();
            return source.Session.Do<T>(source, functionCancellable, infinitTokenSource.Token);
        }

        /// <summary>
        /// Executes the action passed in parameter.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <param name="action">The Action to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used otherwise will run in concurrence of it.</param>
        /// <returns>The current INavigable.</returns>
        public static INavigable Do(this INavigable source, Action<CancellationToken> action, CancellationToken cancellationToken = default)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                source.Session.GlobalCancellationToken,
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            return source.Session.Do(source, action, linkedCts.Token);
        }

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <param name="function">The Function to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used otherwise will run in concurrence of it.</param>
        /// <returns>The expected INavigable returns by the Function.</returns>
        public static INavigable Do<T>(
            this INavigable source,
            Func<CancellationToken, INavigable> function,
            CancellationToken cancellationToken = default) where T : INavigable
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
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The destination.</returns>
        public static INavigable GoTo(
            this INavigable source,
            INavigable destination,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return source.Session.GoTo(source, destination, cancellationToken);
        }

        /// <summary>
        /// Go to the previous INavigable.
        /// </summary>
        /// <param name="source">This Navigable instance.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The previous INavigable.</returns>
        public static INavigable Back(this INavigable source, CancellationToken cancellationToken = default)
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