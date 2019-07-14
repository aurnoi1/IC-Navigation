using System;
using System.Collections.Generic;

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
        /// Executes the UI action passed in parameter.
        /// </summary>
        /// <param name="origin">The INvagable set as origin.</param>
        /// <param name="uIAction">The UI action to execute.</param>
        /// <returns>The expected INavigable which is the same as origin and destination, before and after the UI action invocation.</returns>
        INavigable Do(INavigable origin, Action uIAction);

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <typeparam name="T">The expected returned type of the function that must implement INavigable.</typeparam>
        /// <param name="origin">The INvagable set as origin.</param>
        /// <param name="function">The Function to execute with a declared returned Type.</param>
        /// <returns>The INavigable returns by the Function.</returns>
        INavigable Do<T>(INavigable origin, Func<INavigable> function) where T : INavigable;

        /// <summary>
        /// Get a INavigagble that exists from a List &gt;INavigable&lt; after the UI action is completed.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="onActionAlternatives">The OnActionAlternatives.</param>
        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
        INavigable GetINavigableAfterAction(INavigable origin, IOnActionAlternatives onActionAlternatives);

        /// <summary>
        /// Get INavigable by their attribute UIArtifact.UsageName.
        /// </summary>
        /// <param name="usageName">The expected usage name.</param>
        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
        INavigable GetINavigableByUsageName(string usageName);

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
        INavigable GoTo(INavigable origin, INavigable destination);

        /// <summary>
        /// Performs UI action to step to the next INavigable in the resolve path.
        /// The next INavigable can be a consecutive or rebased to the current INavigable.
        /// </summary>
        /// <param name="actionToOpenInavigable">A Dictionary of UI actions to step to the next Navigable.</param>
        /// <param name="nextNavigable">The next INavigable.</param>
        /// <returns>The next consecutive or the rebased INavigable.</returns>
        /// <exception cref="Exception">The INavigable set as origin was not found."</exception>
        INavigable StepToNext(Dictionary<INavigable, Action> actionToOpenInavigable, INavigable nextNavigable);

        /// <summary>
        /// Back to the previous INavigable.
        /// </summary>
        /// <returns>The previous INavigable.</returns>
        INavigable Back();

        /// <summary>
        /// Resolve a path when one Action leads to more than one page.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative INavigables that can be rebased.</param>
        /// <returns>The destination.</returns>
        INavigable Resolve(INavigable origin, IOnActionAlternatives onActionAlternatives);

        /// <summary>
        /// Resolve a path when one Action leads to more than one INavigable.
        /// This method will search for the first match from OnActionAlternatives list.
        /// An exception will be raised if no path exists between them.
        /// </summary>
        /// <param name="origin">The origin before Action invocation.</param>
        /// <param name="onActionAlternatives">All the alternative INavigables that can be rebased.</param>
        /// <param name="waypoint">An INavigable waypoint to cross before to reach the expected INavigable.</param>
        /// <returns>The destination.</returns>
        INavigable Resolve(INavigable origin, IOnActionAlternatives onActionAlternatives, INavigable waypoint);

        /// <summary>
        /// Compares the Type names of two INavigables.
        /// </summary>
        /// <param name="first">The first INavigable.</param>
        /// <param name="second">The second INavigable.</param>
        /// <returns><c>true</c> if same, otherwise <c>false</c>.</returns>
        bool CompareTypeNames(INavigable first, INavigable second);
    }
}