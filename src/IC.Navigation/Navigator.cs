using IC.Navigation.Exceptions;
using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace IC.Navigation
{
    /// <summary>
    /// An abstract implementation of INavigator.
    /// </summary>
    public class Navigator : INavigator
    {
        #region Fields

        /// <summary>
        /// A temporary field to backup the final destination in GoTo() and used in Resolve().
        /// </summary>
        private INavigable gotoDestination;

        #endregion Fields

        #region Properties

        public IMap Map { get; private set; }
        public ILog Log { get; private set; }

        #endregion Properties

        public Navigator(IMap map, ILog log)
        {
            Map = map;
            Log = log;
        }

        #region Methods

        #region Public

        /// <summary>
        /// Executes the action passed in parameter.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c>then the GlobalCancellationToken will be used.</param>
        /// <returns>The current Navigable.</returns>
        public INavigable Do(
            INavigable navigable,
            Action<CancellationToken> action,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            WaitForReady(navigable, cancellationToken);
            action.Invoke(cancellationToken);
            WaitForExist(navigable, cancellationToken);
            return navigable;
        }

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <typeparam name="T">The expected returned INavigable.</typeparam>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="function">The Function to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c>then the GlobalCancellationToken will be used.</param>
        /// <returns>The Navigable returns by the Function.</returns>
        /// does not implement the expected returned type.</exception>
        public INavigable Do<T>(
            INavigable navigable,
            Func<CancellationToken, INavigable> function,
            CancellationToken cancellationToken) where T : INavigable
        {
            cancellationToken.ThrowIfCancellationRequested();
            WaitForReady(navigable, cancellationToken);
            INavigable retINavigable = function.Invoke(cancellationToken);
            WaitForExist(retINavigable, cancellationToken);
            return retINavigable;
        }

        /// <summary>
        /// Performs action to step to the next Navigable in the resolve path.
        /// The next Navigable can be a consecutive or rebased to the current Navigable.
        /// </summary>
        /// <param name="actionToNextINavigable">A Dictionary of actions to step to the next Navigable.</param>
        /// <param name="next">The next Navigable.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The next Navigable or <see cref="Last"/> if the final destination has been reached
        /// in the action to next Navigable (in case of Resolve() for example).</returns>
        /// <exception cref="UnregistredNeighborException">Throws when next Navigable is not registred in Nodes.</exception>
        public INavigable StepToNext(
            INavigable currentNode,
            INavigable next,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var navigableAndAction = currentNode.GetActionToNext().Where(x => x.Key == next).SingleOrDefault();
            if (navigableAndAction.Key == null)
            {
                throw new UnregistredNeighborException(next, MethodBase.GetCurrentMethod().DeclaringType);
            }

            var actionToOpen = navigableAndAction.Value;
            actionToOpen.Invoke(cancellationToken);
            if (gotoDestination != null)
            {
                var dynamicPath = Map.DynamicNeighbors.Where(x => x.Origin == currentNode).SingleOrDefault();
                if (dynamicPath != null)
                {
                    Resolve(dynamicPath.Alternatives, next, cancellationToken);
                }

                WaitForExist(next, cancellationToken);
                return next;
            }
            else
            {
                return Log.Last; // in case Resolve() was executed in last Invoke, destination is already reached.
            }
        }

        /// <summary>
        /// Go to the destination from the origin, using the shortest way.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The destination.</returns>
        /// <exception cref="UninitializedGraphException">Thrown when the Graph is unitialized.</exception>
        /// <exception cref="PathNotFoundException">Thrown when no path was found between the origin and the destination.</exception>
        public INavigable GoTo(
            INavigable origin,
            INavigable destination,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (Map.Graph == null) { throw new UninitializedGraphException(); }
            WaitForReady(origin, cancellationToken);

            // Avoid calculing the shortest path for the same destination than origin.
            if (origin.ToString() == destination.ToString()) { return destination; }

            var shortestPath = GetShortestPath(origin, destination);
            if (shortestPath.Count == 0)
            {
                throw new PathNotFoundException(origin, destination);
            }

            gotoDestination ??= destination;
            for (int i = 0; i < shortestPath.Count - 1; i++)
            {
                if (gotoDestination != null) // Destination may be already reached via Resolve.
                {
                    var currentNode = shortestPath[i];
                    var nextNode = shortestPath[i + 1];
                    StepToNext(currentNode, nextNode, cancellationToken);
                }
                else
                {
                    break;
                }
            }

            if (gotoDestination == destination)
            {
                gotoDestination = null;
            }

            return destination;
        }

        /// <summary>
        /// Go back to the previous Navigable from <see cref="ILog.Historic"/>.
        /// </summary>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The previous Navigable.</returns>
        public INavigable Back(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return GoTo(Log.Last, Log.Previous, cancellationToken);
        }

        /// <summary>
        /// Get the shortest path from the origin to the destination.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The List of Navigable from the origin to the destination.</returns>
        /// <exception cref="UninitializedGraphException">Thrown when the Graph is unitialized.</exception>
        public List<INavigable> GetShortestPath(INavigable origin, INavigable destination)
        {
            if (Map.Graph == null)
                throw new UninitializedGraphException();

            return Map.Graph.GetShortestPath(origin, destination);
        }

        /// <summary>
        /// Wait until the navigable exists.
        /// </summary>
        /// <param name="navigable">The navigable.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        public void WaitForExist(INavigable navigable, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (navigable.PublishStatus().Exist.Value)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Wait until the navigable is ready.
        /// </summary>
        /// <param name="navigable">The navigable.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        public void WaitForReady(INavigable navigable, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (navigable.PublishStatus().Ready.Value)
                {
                    return;
                }
            }
        }

        #endregion Public

        #region Private

        /// <summary>
        /// Resolve dynamic paths.
        /// </summary>
        /// <param name="alternatives">The alternatives.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="cancellationToken">The cancellation token to interrupt navigation as soon as possible.</param>
        private void Resolve(IEnumerable<INavigable> alternatives, INavigable destination, CancellationToken cancellationToken)
        {
            var alternative = GetFirstINavigableExisting(alternatives, cancellationToken);
            GoTo(alternative, destination, cancellationToken);
        }

        private INavigable GetFirstINavigableExisting(IEnumerable<INavigable> iNavigables, CancellationToken cancellationToken)
        {
            INavigable match = null;
            using (var internalCts = new CancellationTokenSource())
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(internalCts.Token, cancellationToken))
            {
                ParallelOptions po = new ParallelOptions();
                po.CancellationToken = linkedCts.Token;
                try
                {
                    Parallel.ForEach(iNavigables, po, (x, state) =>
                    {
                        var neighbor = GetExistingNavigable(x, po.CancellationToken);
                        if (neighbor != null)
                        {
                            state.Break();
                            match = neighbor;
                            internalCts.Cancel();
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            if (match == null)
            {
                throw new NavigableNotFoundException(iNavigables);
            }

            return match;
        }

        private INavigable GetExistingNavigable(INavigable navigable, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            bool exists = navigable.PublishStatus().Exist.Value;
            return exists ? navigable : null;
        }

        #endregion Private

        #endregion Methods
    }
}