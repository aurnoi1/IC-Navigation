using IC.Navigation.Chain;
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
    public abstract class NavigatorSession : ISession
    {
        #region Fields

        private List<INavigable> historic;

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
        /// Get the nodes formed by instances of INavigables from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing the INavigables.</param>
        /// <returns>Intances of INavigables forming the nodes.</returns>
        public virtual HashSet<INavigable> GetNodesByReflection(Assembly assembly)
        {
            var navigables = new HashSet<INavigable>();
            var iNavigables = assembly.GetTypes()
                .Where(x => typeof(INavigable).IsAssignableFrom(x) && !x.IsInterface)
                .ToList();

            foreach (var iNavigable in iNavigables)
            {
                var instance = Activator.CreateInstance(iNavigable, this) as INavigable;
                navigables.Add(instance);
            }

            return navigables;
        }

        /// <summary>
        /// Get the Graph containing the INavigables.
        /// </summary>
        public abstract IGraph Graph { get; }

        /// <summary>
        /// The INavigables to be expected as entry points when the application start.
        /// </summary>
        public virtual HashSet<INavigable> EntryPoints { get; protected set; }

        /// <summary>
        /// The INavigable EntryPoint that is found at the beginning of the navigation.
        /// Otherwise <c>null</c> if nothing found at the time.
        /// </summary>
        public virtual INavigable EntryPoint => Historic.FirstOrDefault();

        /// <summary>
        /// Multiplicator to adjust the timeout to the environment when waiting for the controls.
        /// </summary>
        public virtual uint ThinkTime { get; set; }

        #endregion Properties

        #region Methods

        #region Public

        /// <summary>
        /// Adjust the timeout to the environment when waiting for the controls depending the <see cref="ThinkTime"/> value.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The adjusted timeout.</returns>
        public virtual TimeSpan AdjustTimeout(TimeSpan timeout)
        {
            var adjTimeout = TimeSpan.FromTicks(timeout.Ticks * ThinkTime);
            return adjTimeout;
        }

        /// <summary>
        /// Wait for any EntryPoints to exists.
        /// The amount of time to wait is defined by each INavigable.WaitForExists().
        /// </summary>
        /// <returns>The first INavigable found, otherwise <c>null</c>.</returns>
        public virtual INavigable WaitForEntryPoints()
        {
            INavigable entryPoint = null;
            Parallel.ForEach(EntryPoints, (iNavigable, state) =>
            {
                if (!state.IsStopped && iNavigable.WaitForExists())
                {
                    entryPoint = iNavigable;
                    state.Stop();
                }
            });

            return entryPoint;
        }

        /// <summary>
        /// Executes the UI action passed in parameter.
        /// </summary>
        /// <param name="origin">The INvagable set as origin.</param>
        /// <param name="uIAction">The UI action to execute.</param>
        /// <returns>The expected INavigable which is the same as origin and destination, before and after the UI action invocation.</returns>
        public virtual INavigable Do(INavigable origin, Action uIAction)
        {
            if (!origin.WaitForExists())
            {
                throw new Exception($"The current INavigagble is not the one expected as origin(\"{origin.ToString()}\").");
            }

            uIAction.Invoke();
            if (!origin.WaitForExists())
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
            var navigableAndAction = actionToNextINavigable.Where(x => x.Key.CompareTypeName(nextNavigable)).SingleOrDefault();
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
                    currentNode.StepToNext(nextNode);
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
            return Last.GoTo(Previous);
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
            return newOrigin.GoTo(gotoDestination);
        }

        /// <summary>
        /// Resolve a path when one Action leads to more than one INavigable.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative INavigables that can be rebased.</param>
        /// <param name="waypoint">An INavigable waypoint to cross before to reach the expected INavigable.</param>
        /// <returns>The destination.</returns>
        public virtual INavigable Resolve(INavigable origin, IOnActionAlternatives onActionAlternatives, INavigable waypoint)
        {
            var newOrigin = GetINavigableAfterAction(origin, onActionAlternatives);
            return newOrigin.GoTo(waypoint).GoTo(gotoDestination);
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
            match = GetFirstINavigableExisting(onActionAlternatives.INavigables).Result;
            return match;
        }

        /// <summary>
        /// Get INavigable by their attribute UIArtefact.UsageName.
        /// </summary>
        /// <param name="usageName">The expected usage name.</param>
        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
        public virtual INavigable GetINavigableByUsageName(string usageName)
        {
            INavigable iNavigable = null;
            foreach (var node in Graph.Nodes)
            {
                var uIArtefact = node.GetType().GetCustomAttribute<UIArtefact>(true);
                if (uIArtefact != null && usageName == uIArtefact.UsageName)
                {
                    iNavigable = node;
                }
            }

            return iNavigable;
        }

        /// <summary>
        /// Comapre two INavigables.
        /// </summary>
        /// <param name="first">First INavigable.</param>
        /// <param name="second">Second INavigable.</param>
        /// <returns><c>true</c> if same. Otherwise <c>false</c>.</returns>
        public virtual bool CompareTypeNames(INavigable first, INavigable second)
        {
            bool equal = first.GetType().Name == second.GetType().Name;
            return equal;
        }

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
        public virtual List<INavigable> Historic
        {
            get
            {
                if (historic == null)
                {
                    historic = new List<INavigable>();
                }

                return historic;
            }

            set
            {
                historic = value;
            }
        }

        /// <summary>
        /// Set the last known INavigable is exists.
        /// </summary>
        /// <param name="iNavigable">The INavigable.</param>
        /// <param name="exists">The result.</param>
        public virtual void SetLast(INavigable iNavigable, bool exists)
        {
            if (exists)
            {
                if (Last == null || !iNavigable.CompareTypeName(Last))
                {
                    Last = iNavigable;
                    OnLastExistingINavigableChanged(
                        Last,
                        new NavigableEventArgs { Exists = exists, Type = Last.GetType() });
                }
            }
        }

        /// <summary>
        /// Event raised when the last known existing INavigable has changed.
        /// </summary>
        public virtual event EventHandler<INavigableEventArgs> ViewChanged;

        #endregion Public

        #region Private

        private async Task<INavigable> GetFirstINavigableExisting(List<INavigable> iNavigables)
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

                        Task<INavigable> completed = await Task.WhenAny(tasks.ToArray());
                        if (completed.Status == TaskStatus.RanToCompletion && completed.Result != null)
                        {
                            return match = completed.Result;
                        }
                        else
                        {
                            counter--;
                        }
                    }

                    // Cancel tasks.
                    token.ThrowIfCancellationRequested();
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
            return navigable.WaitForExists() ? navigable : null;
        }

        private void ValidateINavigableExists(INavigable iNavigagble, string definition)
        {
            if (!iNavigagble.WaitForExists())
            {
                throw new Exception($"The {definition} \"{iNavigagble.ToString()}\" was not found.");
            }
        }

        private void OnLastExistingINavigableChanged(INavigable inavigable, INavigableEventArgs e)
        {
            EventHandler<INavigableEventArgs> handler = ViewChanged;
            handler?.Invoke(inavigable, e);
        }

        #endregion Private

        #endregion Methods
    }
}