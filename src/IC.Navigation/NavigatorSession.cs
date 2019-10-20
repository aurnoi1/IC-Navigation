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
    /// An abstract implementation of INavigator and ISession.
    /// </summary>
    public abstract class NavigatorSession : INavigatorSession
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

        /// <summary>
        /// Lock to queue historic entries.
        /// </summary>
        private readonly object historicLock = new object();

        /// <summary>
        /// Get the Graph containing the INavigables.
        /// </summary>
        public abstract IGraph Graph { get; }

        /// <summary>
        /// The nodes of INavigables forming the Graph.
        /// </summary>
        public abstract HashSet<INavigable> Nodes { get; }

        /// <summary>
        /// The Cancellation Token used to interrupt all the running navigation tasks as soon as possible.
        /// </summary>
        public abstract CancellationToken GlobalCancellationToken { get; set; }

        /// <summary>
        /// The INavigables to be expected as entry points when the application start.
        /// </summary>
        public abstract HashSet<INavigable> EntryPoints { get; protected set; }

        /// <summary>
        /// The INavigable EntryPoint that is found at the beginning of the navigation.
        /// Otherwise <c>null</c> if nothing found at the time.
        /// </summary>
        public virtual INavigable EntryPoint => Historic.FirstOrDefault();

        /// <summary>
        /// Last known INavigable.
        /// </summary>
        public virtual INavigable Last
        {
            get
            {
                lock (historicLock)
                {
                    return Historic.LastOrDefault();
                }
            }

            private set
            {
                lock (historicLock)
                {
                    if (value != null && value != Historic.LastOrDefault())
                    {
                        Historic.Add(value);
                    }
                }
            }
        }

        /// <summary>
        /// Previous accessed INavigable before the last known INavigable.
        /// </summary>
        public virtual INavigable Previous
        {
            get
            {
                lock (historicLock)
                {
                    return Historic.Count > 1 ? Historic[Historic.Count - 2] : null;
                }
            }
        }

        /// The historic of previsous existing INavigable.
        /// </summary>
        public virtual List<INavigable> Historic { get; private set; } = new List<INavigable>();

        #endregion Properties

        #region Methods

        #region Public

        /// <summary>
        /// Get the instance of INavigable from the Nodes.
        /// </summary>
        /// <typeparam name="T">The returned instance type.</typeparam>
        /// <returns>The instance of the requested INavigable.</returns>
        public virtual T GetNavigable<T>() where T : INavigable
        {
            Type type = typeof(T);
            var match = Nodes.Where(n => n.GetType() == type).SingleOrDefault();
            if (match != null)
            {
                return (T)match;
            }
            else
            {
                throw new UnregistredNodeException(type);
            }
        }

        /// <summary>
        /// Get the nodes formed by instances of INavigables from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing the INavigables.</param>
        /// <returns>Intances of INavigables forming the nodes.</returns>
        public virtual HashSet<INavigable> GetNodesByReflection(Assembly assembly)
        {
            var navigables = new HashSet<INavigable>();
            var iNavigables = assembly.GetTypes()
                .Where(x =>
                    typeof(INavigable).IsAssignableFrom(x)
                    && !x.IsInterface
                    && x.IsPublic
                    && !x.IsAbstract
                ).ToList();

            foreach (var iNavigable in iNavigables)
            {
                var instance = Activator.CreateInstance(iNavigable, this) as INavigable;
                navigables.Add(instance);
            }

            return navigables;
        }

        /// <summary>
        /// Wait for any EntryPoints to exists.
        /// The amount of time to wait is defined by each INavigable.WaitForExists().
        /// </summary>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The first INavigable found.</returns>
        /// <exception cref="OperationCanceledException">Throw when the operation has been canceled.</exception>
        /// <exception cref="EntryPointsNotFoundException">Throw when no EntryPoint has been found.</exception>
        public virtual INavigable WaitForEntryPoints(CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            return GetFirstINavigableExisting(EntryPoints.ToList(), localCancellationToken);
        }

        /// <summary>
        /// Wait for any EntryPoints of the navigation to exists.
        /// The amount of time to wait is defined by each INavigable.WaitForExists().
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for any EntryPoints.</param>
        /// <returns>The first INavigable found</returns>
        /// <exception cref="TimeoutException">Throw when timeout is reached before any EntryPoint is found.</exception>
        public INavigable WaitForEntryPoints(TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource(timeout))
            {
                try
                {
                    cts.Token.ThrowIfCancellationRequested();
                    return GetFirstINavigableExisting(EntryPoints.ToList(), cts.Token);
                }
                catch (OperationCanceledException)
                {
                    throw new TimeoutException("The timeout has been reached.");
                }
            }
        }

        /// <summary>
        /// Executes the UI action passed in parameter.
        /// </summary>
        /// <param name="origin">The INvagable set as origin.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The expected INavigable which is the same as origin and destination, before and after the UI action invocation.</returns>
        public virtual INavigable Do(
            INavigable origin, 
            Action<CancellationToken> action, 
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            WaitUntilNavigableExists(origin, "origin", localCancellationToken);
            action.Invoke(localCancellationToken);
            WaitUntilNavigableExists(origin, "origin", localCancellationToken);
            return origin;
        }

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <typeparam name="T">The expected returned type of the function that must implement INavigable.</typeparam>
        /// <param name="origin">The INvagable set as origin.</param>
        /// <param name="function">The Function to execute with a declared returned Type.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The INavigable returns by the Function.</returns>
        /// <exception cref="UnexpectedNavigableException">Thrown when the page after Function invocation 
        /// does not implement the expected returned type.</exception>
        public virtual INavigable Do<T>(
            INavigable origin,
            Func<CancellationToken, INavigable> function,
            CancellationToken cancellationToken = default) where T : INavigable
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            WaitUntilNavigableExists(origin, "origin", localCancellationToken);
            INavigable retINavigable = function.Invoke(localCancellationToken);
            if (!typeof(T).IsAssignableFrom(retINavigable.GetType()))
            {
                throw new UnexpectedNavigableException(typeof(T), retINavigable);
            }

            WaitUntilNavigableExists(retINavigable, "destination", localCancellationToken);
            return retINavigable;
        }

        /// <summary>
        /// Performs UI action to step to the next INavigable in the resolve path.
        /// The next INavigable can be a consecutive or rebased to the current INavigable.
        /// </summary>
        /// <param name="actionToNextINavigable">A Dictionary of UI actions to step to the next Navigable.</param>
        /// <param name="nextNavigable">The next INavigable.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The next INavigable or <see cref="Last"/> if the final destination has been reached
        /// in the action to next INavigable (in case of Resolve() for example). </returns>
        public virtual INavigable StepToNext(
            Dictionary<INavigable,
            Action<CancellationToken>> actionToNextINavigable,
            INavigable nextNavigable,
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            var navigableAndAction = actionToNextINavigable.Where(x => x.Key == nextNavigable).SingleOrDefault();
            if (navigableAndAction.Key == null)
            {
                throw new UnregistredNeighborException(nextNavigable, MethodBase.GetCurrentMethod().DeclaringType);
            }

            var actionToOpen = navigableAndAction.Value;
            actionToOpen.Invoke(localCancellationToken);
            if (gotoDestination != null)
            {
                WaitUntilNavigableExists(nextNavigable, "neighbor to open", localCancellationToken);
                return nextNavigable;
            }
            else
            {
                return Last; // in case Resolve() was executed in last Invoke, destination is already reached.
            }
        }

        /// <summary>
        /// Goto the destination from the origin, using the shortest way to go.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <exception cref="UninitializedGraphException">Thrown when the Graph is unitialized.</exception>
        /// <exception cref="PathNotFoundException">Thrown when no path was found between the origin and the destination.</exception>
        public virtual INavigable GoTo(
            INavigable origin, 
            INavigable destination, 
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            if (Graph == null) { throw new UninitializedGraphException(); }
            WaitUntilNavigableExists(origin, "origin", localCancellationToken);

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
        /// Back to the previous INavigable.
        /// </summary>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The previous INavigable.</returns>
        public virtual INavigable Back(CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            return GoTo(Last, Previous, localCancellationToken);
        }

        /// <summary>
        /// Get the shortest path from the origin to the destination.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The HashSet of INavigable from the origin to the destination.</returns>
        /// <exception cref="UninitializedGraphException">Thrown when the Graph is unitialized.</exception>
        public virtual List<INavigable> GetShortestPath(INavigable origin, INavigable destination)
        {
            if (Graph == null)
                throw new UninitializedGraphException();

            return Graph.GetShortestPath(origin, destination);
        }

        /// <summary>
        /// Resolve a path when one Action leads to more than one page.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative INavigables that can be rebased.</param>
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
            var newOrigin = GetINavigableAfterAction(origin, onActionAlternatives, localCancellationToken);
            return GoTo(newOrigin, gotoDestination, localCancellationToken);
        }

        /// <summary>
        /// Resolve a path when one Action leads to more than one INavigable.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative INavigables that can be rebased.</param>
        /// <param name="waypoint">An INavigable waypoint to cross if the expected INavigable is not cross during the resolution.</param>
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
            var navigableAfterAction = GetINavigableAfterAction(origin, onActionAlternatives, localCancellationToken);
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
        /// Get a INavigagble that exists from a List &gt;INavigable&lt; after the UI action is completed.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="onActionAlternatives">The OnActionAlternatives.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> or <c>null</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
        public virtual INavigable GetINavigableAfterAction(
            INavigable origin,
            IOnActionAlternatives onActionAlternatives,
            CancellationToken cancellationToken = default)
        {
            CancellationToken localCancellationToken = SelectCancellationToken(cancellationToken);
            localCancellationToken.ThrowIfCancellationRequested();
            WaitUntilNavigableExists(origin, "origin", localCancellationToken);
            INavigable match = null;
            onActionAlternatives.AlternativateAction.Invoke(localCancellationToken);
            match = GetFirstINavigableExisting(onActionAlternatives.INavigables, localCancellationToken);
            return match;
        }

        /// <summary>
        /// Update the observer with this Navigable.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="status">The NavigableStatus.</param>
        public virtual void Update(INavigable navigable, INavigableStatus status)
        {
            if (status.Exists)
            {
                SetLast(navigable, status);
            }
        }

        /// <summary>
        /// Publish the historic.
        /// </summary>
        /// <param name="historic">The historic to publish</param>
        public virtual void PublishHistoric(List<INavigable> historic)
        {
            NotifyHistoricObservers(historic);
        }

        /// <summary>
        /// Register HistoricObserver as WeakReference.
        /// </summary>
        /// <param name="observer">The HistoricObserver.</param>
        /// <returns>The WeakReference to the HistoricObserver.</returns>
        public virtual WeakReference<IHistoricObserver> RegisterObserver(IHistoricObserver observer)
        {
            var weakObserver = new WeakReference<IHistoricObserver>(observer);
            observers.Add(weakObserver);
            return weakObserver;
        }

        /// <summary>
        /// Unregister an HistoricObserver.
        /// </summary>
        /// <param name="observer">The HistoricObserver to unregister.</param>
        public virtual void UnregisterObserver(IHistoricObserver observer)
        {
            var obs = observers.Where(x =>
            {
                x.TryGetTarget(out IHistoricObserver target);
                return target.Equals(observer);
            }).SingleOrDefault();

            if (obs != null)
            {
                observers.Remove(obs);
            }
        }

        /// <summary>
        /// Notify HistoricObservers of an update on historic.
        /// </summary>
        /// <param name="historic">The updated historic</param>
        public virtual void NotifyHistoricObservers(List<INavigable> historic)
        {
            observers.ForEach(x =>
            {
                x.TryGetTarget(out IHistoricObserver obs);
                if (obs == null)
                {
                    UnregisterObserver(obs);
                }
                else
                {
                    obs.Update(historic);
                }
            });
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

            if (GlobalCancellationToken == null || GlobalCancellationToken == CancellationToken.None)
            {
                throw new UninitializedGlobalCancellationTokenException();
            }
            else
            {
                return GlobalCancellationToken;
            }
        }

        /// <summary>
        /// Set the last known INavigable is exists.
        /// </summary>
        /// <param name="navigable">The INavigable.</param>
        /// <param name="status">The NavigableStatus of the last INavigable.</param>
        /// <returns><c>true</c> if the INavigable exists, otherwise <c>false</c>.</returns>
        private void SetLast(INavigable navigable, INavigableStatus status)
        {
            if (Last == null || !Equals(navigable, Last))
            {
                Last = navigable;
                PublishHistoric(Historic);
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
                throw new EntryPointsNotFoundException(iNavigables);
            }

            return match;
        }

        private INavigable GetExistingNavigable(INavigable navigable, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            bool exists = navigable.PublishStatus().Exists;
            return exists ? navigable : null;
        }

        private void WaitUntilNavigableExists(INavigable iNavigable, string definition, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (iNavigable.PublishStatus().Exists)
                {
                    return;
                }
            }
        }

        #endregion Private

        #endregion Methods
    }
}