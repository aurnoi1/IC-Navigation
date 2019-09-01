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
        /// The INavigables to be expected as entry points when the application start.
        /// </summary>
        public abstract HashSet<INavigable> EntryPoints { get; protected set; }

        /// <summary>
        /// Positive multiplier to adjust the timeouts when waiting for UI objects.
        /// </summary>
        public abstract double ThinkTime { get; set; }

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

        /// <summary>
        /// The historic of previsous existing INavigable.
        /// </summary>
        public virtual List<INavigable> Historic { get; private set; } = new List<INavigable>();

        /// <summary>
        /// Event raised when the last known existing INavigable has changed in Historic.
        /// </summary>
        public virtual event EventHandler<INavigableEventArgs> HistoricChanged;

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
                throw new Exception($"\"{type}\" is not part of the Nodes.");
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
        /// Adjust the timeout when waiting for the UI objects depending the <see cref="ThinkTime"/> value.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The adjusted timeout.</returns>
        public virtual TimeSpan AdjustTimeout(TimeSpan timeout)
        {
            var adjTimeout = TimeSpan.FromTicks(timeout.Ticks * Convert.ToInt64(ThinkTime));
            return adjTimeout;
        }

        /// <summary>
        /// Wait for any EntryPoints to exists.
        /// The amount of time to wait is defined by each INavigable.WaitForExists().
        /// </summary>
        /// <returns>The first INavigable found, otherwise <c>null</c>.</returns>
        public virtual INavigable WaitForEntryPoints()
        {
            return GetFirstINavigableExisting(EntryPoints.ToList());
        }

        /// <summary>
        /// Executes the UI action passed in parameter.
        /// </summary>
        /// <param name="origin">The INvagable set as origin.</param>
        /// <param name="uIAction">The UI action to execute.</param>
        /// <returns>The expected INavigable which is the same as origin and destination, before and after the UI action invocation.</returns>
        public virtual INavigable Do(INavigable origin, Action uIAction)
        {
            if (!origin.PublishExistsStatus())
            {
                throw new Exception($"The current INavigagble is not the one expected as origin(\"{origin.ToString()}\").");
            }

            uIAction.Invoke();
            if (!origin.PublishExistsStatus())
            {
                throw new Exception($"The current INavigagble is not the same than expected (\"{origin.ToString()}\")." +
                    $" If it was expected, used \"Do<T>\" instead.");
            }

            return origin;
        }

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <typeparam name="T">The expected returned type of the function that must implement INavigable.</typeparam>
        /// <param name="origin">The INvagable set as origin.</param>
        /// <param name="function">The Function to execute with a declared returned Type.</param>
        /// <returns>The INavigable returns by the Function.</returns>
        public virtual INavigable Do<T>(INavigable origin, Func<INavigable> function) where T : INavigable
        {
            ValidateINavigableExists(origin, "origin");
            INavigable retINavigable = function.Invoke();
            var actualINavigable = retINavigable.GetType();
            Type expectedINavigable = typeof(T);
            if (typeof(T) != retINavigable.GetType())
            {
                throw new Exception($"The expected INavigagble Type is \"{expectedINavigable}\" but the actual INavigagble Type is \"{actualINavigable}\"");
            }

            ValidateINavigableExists(retINavigable, "destination");
            return retINavigable;
        }

        /// <summary>
        /// Performs UI action to step to the next INavigable in the resolve path.
        /// The next INavigable can be a consecutive or rebased to the current INavigable.
        /// </summary>
        /// <param name="actionToNextINavigable">A Dictionary of UI actions to step to the next Navigable.</param>
        /// <param name="nextNavigable">The next INavigable.</param>
        /// <returns>The next INavigable or <see cref="Last"/> if the final destination has been reached
        /// in the action to next INavigable (in case of Resolve() for example). </returns>
        /// <exception cref="Exception">The INavigable set as origin was not found."</exception>
        public virtual INavigable StepToNext(Dictionary<INavigable, Action> actionToNextINavigable, INavigable nextNavigable)
        {
            var navigableAndAction = actionToNextINavigable.Where(x => x.Key == nextNavigable).SingleOrDefault();
            INavigable nextNavigableRef = navigableAndAction.Key;
            Action actionToOpen = navigableAndAction.Value;
            if (nextNavigableRef == null)
            {
                throw new ArgumentException($"The INavigable \"{nextNavigable}\" is not available in \"{MethodBase.GetCurrentMethod().DeclaringType}\".");
            }

            actionToOpen.Invoke();
            if (gotoDestination != null)
            {
                ValidateINavigableExists(nextNavigable, "neighbor to open");
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
        /// <returns>The destination.</returns>
        public virtual INavigable GoTo(INavigable origin, INavigable destination)
        {
            if (Graph == null) { throw new Exception($"The \"Graph\" is not initialized."); }

            ValidateINavigableExists(origin, "origin");

            // Avoid calculing the shortest path for the same destination than origin.
            if (origin.ToString() == destination.ToString()) { return destination; }

            var shortestPath = GetShortestPath(origin, destination);
            if (shortestPath.Count == 0)
            {
                throw new Exception($"There is no path from \"{origin.GetType().Name}\" to \"{destination.GetType().Name}\".");
            }

            gotoDestination = gotoDestination ?? destination;
            for (int i = 0; i < shortestPath.Count - 1; i++)
            {
                if (gotoDestination != null) // Destination may be already reached via Resolve.
                {
                    var currentNode = shortestPath[i];
                    var nextNode = shortestPath[i + 1];
                    StepToNext(currentNode.GetActionToNext(), nextNode);
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
        /// <returns>The previous INavigable.</returns>
        public virtual INavigable Back()
        {
            return GoTo(Last, Previous);
        }

        /// <summary>
        /// Get the shortest path from the origin to the destination.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The HashSet of INavigable from the origin to the destination.</returns>
        public virtual List<INavigable> GetShortestPath(INavigable origin, INavigable destination)
        {
            return Graph.GetShortestPath(origin, destination);
        }

        /// <summary>
        /// Resolve a path when one Action leads to more than one page.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative INavigables that can be rebased.</param>
        /// <returns>The destination.</returns>
        public virtual INavigable Resolve(INavigable origin, IOnActionAlternatives onActionAlternatives)
        {
            var newOrigin = GetINavigableAfterAction(origin, onActionAlternatives);
            return GoTo(newOrigin, gotoDestination);
        }

        /// <summary>
        /// Resolve a path when one Action leads to more than one INavigable.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative INavigables that can be rebased.</param>
        /// <param name="waypoint">An INavigable waypoint to cross if the expected INavigable is not cross during the resolution.</param>
        /// <returns>The destination.</returns>
        public virtual INavigable Resolve(INavigable origin, IOnActionAlternatives onActionAlternatives, INavigable waypoint)
        {
            // gotoDestination will be reset with the first call to GoTo().
            var finalDestination = gotoDestination;
            var navigableAfterAction = GetINavigableAfterAction(origin, onActionAlternatives);
            if (navigableAfterAction == finalDestination)
            {
                return navigableAfterAction;
            }
            else
            {
                // Force to pass by waypoint.
                GoTo(navigableAfterAction, waypoint);
                return GoTo(waypoint, finalDestination);
            }
        }

        /// <summary>
        /// Get a INavigagble that exists from a List &gt;INavigable&lt; after the UI action is completed.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="onActionAlternatives">The OnActionAlternatives.</param>
        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
        public virtual INavigable GetINavigableAfterAction(INavigable origin, IOnActionAlternatives onActionAlternatives)
        {
            ValidateINavigableExists(origin, "origin");
            INavigable match = null;
            onActionAlternatives.UIAction.Invoke();
            match = GetFirstINavigableExisting(onActionAlternatives.INavigables);
            return match;
        }

        public void Update(INavigable navigable, INavigableEventArgs args)
        {
            if (args.Exists)
            {
                SetLast(navigable);
            }
        }

        #endregion Public

        #region Private

        /// <summary>
        /// Set the last known INavigable is exists.
        /// </summary>
        /// <param name="navigable">The INavigable.</param>
        /// <returns><c>true</c> if the INavigable exists, otherwise <c>false</c>.</returns>
        private void SetLast(INavigable navigable)
        {
            if (Last == null || !Equals(navigable, Last))
            {
                Last = navigable;
                var eventArgs = new NavigableEventArgs() { Exists = true };
                OnHistoricChanged(navigable, eventArgs);
            }
        }

        private INavigable GetFirstINavigableExisting(IEnumerable<INavigable> iNavigables)
        {
            INavigable match = null;
            int counter = iNavigables.Count();
            List<Task<INavigable>> tasks = new List<Task<INavigable>>();
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                try
                {
                    CancellationToken token = source.Token;
                    foreach (var iNavigagble in iNavigables)
                    {
                        tasks.Add(new Task<INavigable>(() => WaitForExists(iNavigagble), token));
                    }

                    bool tasksStarted = false;
                    while (counter > 0)
                    {
                        if (!tasksStarted)
                        {
                            tasks.ForEach(x => x.Start());
                            tasksStarted = true;
                        }

                        Task<INavigable> completed = Task.WhenAny(tasks.ToArray()).GetAwaiter().GetResult();
                        if (completed.Status == TaskStatus.RanToCompletion && completed.Result != null)
                        {
                            match = completed.Result;
                            counter = 0;
                            source.Cancel();
                            break;
                        }
                        else
                        {
                            counter--;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Do nothing.
                }
            }

            return match;
        }

        private INavigable WaitForExists(INavigable navigable)
        {
            bool exists = navigable.PublishExistsStatus();
            return exists ? navigable : null;
        }

        private void ValidateINavigableExists(INavigable iNavigable, string definition)
        {
            if (!iNavigable.PublishExistsStatus())
            {
                throw new Exception($"The {definition} \"{iNavigable.ToString()}\" was not found.");
            }
        }

        private void OnHistoricChanged(INavigable inavigable, INavigableEventArgs e)
        {
            EventHandler<INavigableEventArgs> handler = HistoricChanged;
            handler?.Invoke(inavigable, e);
        }

        #endregion Private

        #endregion Methods
    }
}