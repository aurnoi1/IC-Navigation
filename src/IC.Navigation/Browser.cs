using IC.Navigation.Interfaces;
using System;
using System.Threading;

namespace IC.Navigation
{
    public class Browser : IBrowser
    {
        public IMap Map { get; }

        public INavigator Navigator { get; }
        public ILog Log { get; }

        public Browser(IMap map, ILog log, INavigator navigator)
        {
            Map = map;
            Log = log;
            Navigator = navigator;
        }

        /// <summary>
        /// Executes the action passed in parameter on the last Navigable.
        /// </summary>
        /// <param name="action">The action to execute.</param>m>
        /// <returns>This Browser.</returns>
        public IBrowser Do(Action action)
        {
            using var infinitTokenSource = new CancellationTokenSource();
            var last = Log.Last;
            void actionNotCancellable(CancellationToken ct) => action();
            Navigator.Do(last, actionNotCancellable, infinitTokenSource.Token);
            return this;
        }

        /// <summary>
        /// Executes the Function passed in parameter on the last Navigable.
        /// </summary>
        /// <param name="function">The non-cancellable Function to execute.</param>
        /// <returns>This Browser.</returns>
        public IBrowser Do<T>(
            Func<INavigable> function) where T : INavigable
        {
            using var infinitTokenSource = new CancellationTokenSource();
            var last = Log.Last;
            INavigable functionNotCancellable(CancellationToken ct) => function();
            Navigator.Do<T>(last, functionNotCancellable, infinitTokenSource.Token);
            return this;
        }

        /// <summary>
        /// Executes the action passed in parameter on the last Navigable.
        /// </summary>
        /// <param name="action">The Action to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used otherwise will run in concurrence of it.</param>
        /// <returns>This Browser.</returns>
        public IBrowser Do(
            Action<CancellationToken> action,
            CancellationToken cancellationToken = default)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                Map.GlobalCancellationToken,
                cancellationToken);

            var last = Log.Last;
            linkedCts.Token.ThrowIfCancellationRequested();
            Navigator.Do(last, action, linkedCts.Token);
            return this;
        }

        /// <summary>
        /// Executes the Function passed in parameter on the last Navigable.
        /// </summary>
        /// <param name="function">The Function to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used otherwise will run in concurrence of it.</param>
        /// <returns>This Browser.</returns>
        public IBrowser Do<T>(
            Func<CancellationToken, INavigable> function,
            CancellationToken cancellationToken = default) where T : INavigable
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                Map.GlobalCancellationToken,
                cancellationToken);

            linkedCts.Token.ThrowIfCancellationRequested();
            var last = Log.Last;
            Navigator.Do<T>(last, function, linkedCts.Token);
            return this;
        }

        /// <summary>
        /// Go to the destination from the last Navigable, using the shortest way.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>This Browser.</returns>
        /// <exception cref="UninitializedGraphException">Thrown when the Graph is unitialized.</exception>
        /// <exception cref="PathNotFoundException">Thrown when no path was found between the origin and the destination.</exception>
        public IBrowser GoTo(
                INavigable destination,
                CancellationToken cancellationToken = default)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                Map.GlobalCancellationToken,
                cancellationToken);

            linkedCts.Token.ThrowIfCancellationRequested();
            var last = Log.Last;
            Navigator.GoTo(last, destination, linkedCts.Token);
            return this;
        }

        /// <summary>
        /// Go back to the previous Navigable from <see cref="ILog.Historic"/>.
        /// </summary>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The previous Navigable.</returns>
        public IBrowser Back(CancellationToken cancellationToken = default)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                Map.GlobalCancellationToken,
                cancellationToken);

            linkedCts.Token.ThrowIfCancellationRequested();
            Navigator.Back(linkedCts.Token);
            return this;
        }

        /// <summary>
        /// Get the <see cref="INavigableStatus.Exist"/> status of this Navigable./>.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <returns><c>true</c> if exists, otherwise <c>false</c>.</returns>
        public bool Exists(INavigable navigable)
        {
            return navigable.PublishStatus().Exist.Value;
        }

        /// <summary>
        /// Wait until this navigable exists.
        /// </summary>
        /// <param name="navigable">the Navigable.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns><c>true</c> if exists before the CancellationToken is canceled.
        /// Otherwise <c>false</c>.</returns>
        public bool WaitForExist(INavigable navigable, CancellationToken cancellationToken)
        {
            Navigator.WaitForExist(navigable, cancellationToken);
            return !cancellationToken.IsCancellationRequested;
        }

        /// <summary>
        /// Wait until this navigable is ready.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns><c>true</c> if ready before the CancellationToken is canceled.
        /// Otherwise <c>false</c>.</returns>
        public bool WaitForReady(INavigable navigable, CancellationToken cancellationToken)
        {
            Navigator.WaitForReady(navigable, cancellationToken);
            return !cancellationToken.IsCancellationRequested;
        }
    }
}