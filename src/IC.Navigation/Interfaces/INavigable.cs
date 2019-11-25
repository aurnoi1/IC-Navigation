using IC.Navigation.Enums;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines an INavigable.
    /// </summary>
    public interface INavigable
    {
        /// <summary>
        /// The navigation session.
        /// </summary>
        INavigator Navigator { get; }

        /// <summary>
        /// Notify observers of the current INavigable status.
        /// </summary>
        /// <returns>The published current INavigable status.</returns>
        INavigableStatus PublishStatus();

        /// <summary>
        /// Notify observers of a specific State's value.
        /// </summary>
        /// <typeparam name="T">The State's value type.</typeparam>
        /// <param name="stateName">The state name.</param>
        /// <returns>The State.</returns>
        IState<T> PublishState<T>(StatesNames stateName);

        /// <summary>
        /// Gets a Dictionary of action to go to the next INavigable.
        /// </summary>
        /// <returns>A Dictionary of action to go to the next INavigable.</returns>
        Dictionary<INavigable, Action<CancellationToken>> GetActionToNext();

        /// <summary>
        /// Register the NavigableObserver as a WeakReference.
        /// </summary>
        /// <param name="observer">The INavigableObserver.</param>
        /// <returns>The INavigableObserver as a WeakReference.</returns>
        WeakReference<INavigableObserver> RegisterObserver(INavigableObserver observer);

        /// <summary>
        /// Unregister the NavigableObserver.
        /// </summary>
        /// <param name="weakObserver">The INavigableObserver.</param>
        void UnregisterObserver(INavigableObserver observer);

        /// <summary>
        /// Notify all observers of the current NavigableStatus.
        /// </summary>
        /// <param name="status">The NavigableStatus.</param>
        void NotifyObservers(INavigableStatus status);

        /// <summary>
        /// Notify all observers of the current state.
        /// </summary>
        /// <param name="state">The State.</param>
        void NotifyObservers<T>(IState<T> state);
    }
}