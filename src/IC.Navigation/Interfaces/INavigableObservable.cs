using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines an INavigableObservable.
    /// </summary>
    public interface INavigableObservable
    {
        /// <summary>
        /// Register the INavigableObserver as a WeakReference.
        /// </summary>
        /// <param name="observer">The INavigableObserver.</param>
        /// <returns>The INavigableObserver as a WeakReference.</returns>
        WeakReference<INavigableObserver> RegisterObserver(INavigableObserver observer);

        /// <summary>
        /// Unregister the INavigableObserver.
        /// </summary>
        /// <param name="weakObserver">The INavigableObserver as a WeakReference.</param>
        void UnregisterObserver(WeakReference<INavigableObserver> weakObserver);

        /// <summary>
        /// Notify all observers.
        /// </summary>
        /// <param name="args">The INavigableEventArgs.</param>
        void NotifyObservers(INavigableEventArgs args);
    }
}
