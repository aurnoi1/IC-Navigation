﻿using IC.Navigation.Exceptions;
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
    public abstract class Navigator : INavigator
    {
        #region Fields

        /// <summary>
        /// A temporary field to backup the final destination in GoTo() and used in Resolve().
        /// </summary>
        private INavigable gotoDestination;

        /// <summary>
        /// The WeakReferences to HistoricObservers.
        /// </summary>
        private readonly List<WeakReference<IHistoricObserver>> observers = new List<WeakReference<IHistoricObserver>>();

        #endregion Fields

        #region Properties

        public abstract IMap Map { get; set; }

        #endregion Properties

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
        public virtual INavigable Do(
            INavigable navigable,
            Action<CancellationToken> action,
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            WaitForReady(navigable, localCancellationToken);
            action.Invoke(localCancellationToken);
            WaitForExist(navigable, localCancellationToken);
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
        public virtual INavigable Do<T>(
            INavigable navigable,
            Func<CancellationToken, INavigable> function,
            CancellationToken cancellationToken = default) where T : INavigable
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            WaitForReady(navigable, localCancellationToken);
            INavigable retINavigable = function.Invoke(localCancellationToken);
            WaitForExist(retINavigable, localCancellationToken);
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
        public virtual INavigable StepToNext(
            Dictionary<INavigable,
            Action<CancellationToken>> actionToNextINavigable,
            INavigable next,
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            var navigableAndAction = actionToNextINavigable.Where(x => x.Key == next).SingleOrDefault();
            if (navigableAndAction.Key == null)
            {
                throw new UnregistredNeighborException(next, MethodBase.GetCurrentMethod().DeclaringType);
            }

            var actionToOpen = navigableAndAction.Value;
            actionToOpen.Invoke(localCancellationToken);
            if (gotoDestination != null)
            {
                WaitForExist(next, localCancellationToken);
                return next;
            }
            else
            {
                return Map.Log.Last; // in case Resolve() was executed in last Invoke, destination is already reached.
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
        public virtual INavigable GoTo(
            INavigable origin,
            INavigable destination,
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            if (Map.Graph == null) { throw new UninitializedGraphException(); }
            WaitForReady(origin, localCancellationToken);

            // Avoid calculing the shortest path for the same destination than origin.
            if (origin.ToString() == destination.ToString()) { return destination; }

            var shortestPath = GetShortestPath(origin, destination);
            if (shortestPath.Count == 0)
            {
                throw new PathNotFoundException(origin, destination);
            }

            gotoDestination = gotoDestination ?? destination;
            for (int i = 0; i < shortestPath.Count - 1; i++)
            {
                if (gotoDestination != null) // Destination may be already reached via Resolve.
                {
                    var currentNode = shortestPath[i];
                    var nextNode = shortestPath[i + 1];
                    StepToNext(currentNode.GetActionToNext(), nextNode, localCancellationToken);
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
        public virtual INavigable Back(CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            return GoTo(Map.Log.Last, Map.Log.Previous, localCancellationToken);
        }

        /// <summary>
        /// Get the shortest path from the origin to the destination.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The List of Navigable from the origin to the destination.</returns>
        /// <exception cref="UninitializedGraphException">Thrown when the Graph is unitialized.</exception>
        public virtual List<INavigable> GetShortestPath(INavigable origin, INavigable destination)
        {
            if (Map.Graph == null)
                throw new UninitializedGraphException();

            return Map.Graph.GetShortestPath(origin, destination);
        }

        /// <summary>
        /// Resolve a path when one action leads to more than one Navigable.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative Navigables that can be rebased.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The destination.</returns>
        public virtual INavigable Resolve(
            INavigable origin,
            IOnActionAlternatives onActionAlternatives,
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            var newOrigin = GetNavigableAfterAction(origin, onActionAlternatives, localCancellationToken);
            return GoTo(newOrigin, gotoDestination, localCancellationToken);
        }

        /// <summary>
        /// Resolve a path when one action leads to more than one Navigable.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative Navigables that can be rebased.</param>
        /// <param name="waypoint">An Navigable waypoint to cross before to reach the expected INavigable.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The destination.</returns>
        public virtual INavigable Resolve(
            INavigable origin,
            IOnActionAlternatives onActionAlternatives,
            INavigable waypoint,
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();

            // gotoDestination will be reset with the first call to GoTo().
            var finalDestination = gotoDestination;
            var navigableAfterAction = GetNavigableAfterAction(origin, onActionAlternatives, localCancellationToken);
            if (navigableAfterAction == finalDestination)
            {
                return navigableAfterAction;
            }
            else
            {
                // Force to pass by waypoint.
                GoTo(navigableAfterAction, waypoint, localCancellationToken);
                return GoTo(waypoint, finalDestination, localCancellationToken);
            }
        }

        /// <summary>
        /// Get the Navigagble that exists after the OnActionAlternatives's action is completed.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="onActionAlternatives">The OnActionAlternatives.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The matching Navigable, otherwise <c>null</c>.</returns>
        public virtual INavigable GetNavigableAfterAction(
            INavigable navigable,
            IOnActionAlternatives onActionAlternatives,
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            WaitForExist(navigable, localCancellationToken);
            INavigable match = null;
            onActionAlternatives.AlternativateAction.Invoke(localCancellationToken);
            match = GetFirstINavigableExisting(onActionAlternatives.Navigables, localCancellationToken);
            return match;
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
        /// Select the CancellationToken to use for a task.
        /// If the local token is not <c>None</c> or <c>null</c> it will we used,
        /// otherwise the GlobalCancellationToken will be used.
        /// </summary>
        /// <param name="localToken"></param>
        /// <returns></returns>
        /// <remarks>The localToken may contains the GlobalCancellationToken if its source has been linked.</remarks>
        /// <exception cref="UninitializedGlobalCancellationTokenException">Thrown when the GlobalCancellationToken is unitialized.</exception>
        private CancellationToken SelectCancellationToken(CancellationToken localToken)
        {
            localToken.ThrowIfCancellationRequested();
            if (localToken != null && localToken != CancellationToken.None)
            {
                return localToken;
            }

            if (Map.GlobalCancellationToken == null || Map.GlobalCancellationToken == CancellationToken.None)
            {
                throw new UninitializedGlobalCancellationTokenException();
            }
            else
            {
                return Map.GlobalCancellationToken;
            }
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