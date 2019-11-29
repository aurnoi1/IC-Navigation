using IC.Navigation.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines a Navigator to navigate through Graph.
    /// </summary>
    public interface INavigator
    {
        IMap Map { get;}
        ILog Log { get; }

        CancellationToken GlobalCancellationToken { get; }

        /// <summary>
        /// Executes the action passed in parameter.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c>then the GlobalCancellationToken will be used.</param>
        /// <returns>The current Navigable.</returns>
        INavigable Do(INavigable navigable, Action<CancellationToken> action, CancellationToken cancellationToken);

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <typeparam name="T">The expected returned INavigable.</typeparam>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="function">The Function to execute.</param>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c>then the GlobalCancellationToken will be used.</param>
        /// <returns>The Navigable returns by the Function.</returns>
        INavigable Do<T>(INavigable navigable, Func<CancellationToken, INavigable> function, CancellationToken cancellationToken) where T : INavigable;

        /// <summary>
        /// Get the shortest path from the origin to the destination.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The List of Navigable from the origin to the destination.</returns>
        /// <exception cref="UninitializedGraphException">Thrown when the Graph is unitialized.</exception>
        List<INavigable> GetShortestPath(INavigable origin, INavigable destination);

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
        INavigable GoTo(INavigable origin, INavigable destination, CancellationToken cancellationToken);

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
        INavigable StepToNext(
            INavigable origin,
            INavigable next,
            CancellationToken cancellationToken);

        /// <summary>
        /// Go back to the previous Navigable from <see cref="ILog.Historic"/>.
        /// </summary>
        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
        /// <returns>The previous Navigable.</returns>
        INavigable Back(CancellationToken cancellationToken);

        /// <summary>
        /// Wait until the Navigable exists.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        void WaitForExist(INavigable navigable, CancellationToken cancellationToken);

        /// <summary>
        /// Wait until the Navigable is ready.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        void WaitForReady(INavigable navigable, CancellationToken cancellationToken);
    }
}