﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines a Navigator to navigate through Graph.
    /// </summary>
    public interface INavigator : ILog
    {
        /// <summary>
        /// Get the Graph containing the INavigables.
        /// </summary>
        IGraph Graph { get; }

        /// <summary>
        /// The nodes of INavigables forming the Graph.
        /// </summary>
        HashSet<INavigable> Nodes { get; }

        /// <summary>
        /// The Cancellation Token used to interrupt all the running navigation tasks as soon as possible.
        /// </summary>
        CancellationToken GlobalCancellationToken { get; set; }

        /// <summary>
        /// Executes the UI action passed in parameter.
        /// </summary>
        /// <param name="origin">The INvagable set as origin.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The expected INavigable which is the same as origin and destination, before and after the UI action invocation.</returns>
        INavigable Do(INavigable origin, Action action, CancellationToken cancellationToken);

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <typeparam name="T">The expected returned type of the function that must implement INavigable.</typeparam>
        /// <param name="origin">The INvagable set as origin.</param>
        /// <param name="function">The Function to execute with a declared returned Type.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The INavigable returns by the Function.</returns>
        INavigable Do<T>(INavigable origin, Func<INavigable> function, CancellationToken cancellationToken) where T : INavigable;

        /// <summary>
        /// Get a INavigagble that exists from a List &gt;INavigable&lt; after the UI action is completed.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="onActionAlternatives">The OnActionAlternatives.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
        INavigable GetINavigableAfterAction(INavigable origin, IOnActionAlternatives onActionAlternatives, CancellationToken cancellationToken);

        /// <summary>
        /// Get the shortest path from the origin to the destination.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The List of INavigable from the origin to the destination.</returns>
        List<INavigable> GetShortestPath(INavigable origin, INavigable destination);

        /// <summary>
        /// Goto the destination from the origin, using the shortest way to go.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The destination.</returns>
        INavigable GoTo(INavigable origin, INavigable destination, CancellationToken cancellationToken);

        /// <summary>
        /// Performs UI action to step to the next INavigable in the resolve path.
        /// The next INavigable can be a consecutive or rebased to the current INavigable.
        /// </summary>
        /// <param name="actionToOpenInavigable">A Dictionary of UI actions to step to the next Navigable.</param>
        /// <param name="nextNavigable">The next INavigable.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The next consecutive or the rebased INavigable.</returns>
        /// <exception cref="Exception">The INavigable set as origin was not found."</exception>
        INavigable StepToNext(
            Dictionary<INavigable, Action<CancellationToken>> actionToOpenInavigable, 
            INavigable nextNavigable, 
            CancellationToken cancellationToken);

        /// <summary>
        /// Back to the previous INavigable.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The previous INavigable.</returns>
        INavigable Back(CancellationToken cancellationToken);

        /// <summary>
        /// Resolve a path when one Action leads to more than one page.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative INavigables that can be rebased.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The destination.</returns>
        INavigable Resolve(INavigable origin, IOnActionAlternatives onActionAlternatives, CancellationToken cancellationToken);

        /// <summary>
        /// Resolve a path when one Action leads to more than one INavigable.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative INavigables that can be rebased.</param>
        /// <param name="waypoint">An INavigable waypoint to cross before to reach the expected INavigable.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The destination.</returns>
        INavigable Resolve(
            INavigable origin, 
            IOnActionAlternatives onActionAlternatives, 
            INavigable waypoint, 
            CancellationToken cancellationToken);
    }
}