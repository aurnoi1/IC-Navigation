using System;
using System.Collections.Generic;

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
        ISession Session { get; }

        /// <summary>
        /// Notify observers of the current INavigable status.
        /// </summary>
        /// <returns>The published current INavigable status.</returns>
        INavigableStatus PublishStatus();

        /// <summary>
        /// Gets a Dictionary of action to go to the next INavigable.
        /// </summary>
        /// <returns>A Dictionary of action to go to the next INavigable.</returns>
        Dictionary<INavigable, Action> GetActionToNext();

        /// <summary>
        /// Register the INavigableObserver as a WeakReference.
        /// </summary>
        /// <param name="observer">The INavigableObserver.</param>
        /// <returns>The INavigableObserver as a WeakReference.</returns>
        WeakReference<INavigableObserver> RegisterObserver(INavigableObserver observer);

        /// <summary>
        /// Unregister the INavigableObserver.
        /// </summary>
        /// <param name="weakObserver">The INavigableObserver.</param>
        void UnregisterObserver(INavigableObserver observer);

        /// <summary>
        /// Notify all observers.
        /// </summary>
        /// <param name="status">The NavigableStatus.</param>
        void NotifyObservers(INavigableStatus status);

    }
}