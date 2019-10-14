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
        /// <param name="origin">This Navigable instance.</param>
        /// <param name="action">The non-cancellable Action to execute.</param>
        /// <returns>The current INavigable.</returns>
        public static INavigable Do(this INavigable origin, Action action)
        {
            using var infinitTokenSource = new CancellationTokenSource();
            void actionNotCancellable(CancellationToken ct) => action();
            return origin.Session.Do(origin, actionNotCancellable, infinitTokenSource.Token);
        }

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <param name="origin">This Navigable instance.</param>
        /// <param name="function">The non-cancellable Function to execute.</param>
        /// <returns>The expected INavigable returns by the Function.</returns>
        public static INavigable Do<T>(
            this INavigable origin,
            Func<INavigable> function) where T : INavigable
        {
            using var infinitTokenSource = new CancellationTokenSource();
            INavigable functionNotCancellable(CancellationToken ct) => function();
            return origin.Session.Do<T>(origin, functionNotCancellable, infinitTokenSource.Token);
        }

        /// <summary>
        /// Executes the action passed in parameter.
        /// </summary>
        /// <param name="origin">This Navigable instance.</param>
        /// <param name="action">The Action to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used otherwise will run in concurrence of it.</param>
        /// <returns>The current INavigable.</returns>
        public static INavigable Do(this INavigable origin, Action<CancellationToken> action, CancellationToken cancellationToken = default)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                origin.Session.GlobalCancellationToken,
                cancellationToken);

            linkedCts.Token.ThrowIfCancellationRequested();
            return origin.Session.Do(origin, action, linkedCts.Token);
        }

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <param name="origin">This Navigable instance.</param>
        /// <param name="function">The Function to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used otherwise will run in concurrence of it.</param>
        /// <returns>The expected INavigable returns by the Function.</returns>
        public static INavigable Do<T>(
            this INavigable origin,
            Func<CancellationToken, INavigable> function,
            CancellationToken cancellationToken = default) where T : INavigable
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                origin.Session.GlobalCancellationToken,
                cancellationToken);

            linkedCts.Token.ThrowIfCancellationRequested();
            return origin.Session.Do<T>(origin, function, linkedCts.Token);
        }

        /// <summary>
        /// Find the shortest path of navigation to go to the destination INavigable,
        /// then performs actions through other INavigables.
        /// </summary>
        /// <param name="origin">This Navigable instance.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The destination.</returns>
        public static INavigable GoTo(
                this INavigable origin,
                INavigable destination,
                CancellationToken cancellationToken = default)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                origin.Session.GlobalCancellationToken,
                cancellationToken);

            linkedCts.Token.ThrowIfCancellationRequested();
            return origin.Session.GoTo(origin, destination, linkedCts.Token);
        }

        /// <summary>
        /// Go to the previous INavigable.
        /// </summary>
        /// <param name="origin">This Navigable instance.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The previous INavigable.</returns>
        public static INavigable Back(this INavigable origin, CancellationToken cancellationToken = default)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                origin.Session.GlobalCancellationToken,
                cancellationToken);

            linkedCts.Token.ThrowIfCancellationRequested();
            return origin.Session.Back(linkedCts.Token);
        }

        /// <summary>
        /// Get the <see cref="INavigableStatus.Exists"/> status of the Navigable./>.
        /// </summary>
        /// <param name="origin">This Navigable instance.</param>
        /// <returns><c>true</c> if exists, otherwise <c>false</c>.</returns>
        public static bool Exists(this INavigable origin)
        {
            return origin.PublishStatus().Exists;
        }
    }
}