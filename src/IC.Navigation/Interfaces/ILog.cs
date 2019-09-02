using System;
using System.Collections.Generic;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Represents the implementation of ILog.
    /// </summary>
    public interface ILog : INavigableObserver
    {
        /// <summary>
        /// Last known existing INavigable.
        /// </summary>
        INavigable Last { get; }

        /// <summary>
        /// Previous accessed INavigable before the last known INavigable.
        /// </summary>
        INavigable Previous { get; }

        /// <summary>
        /// The historic of previsous existing INavigable.
        /// </summary>
        List<INavigable> Historic { get; }

        /// <summary>
        /// Publish the historic.
        /// </summary>
        /// <param name="historic">The historic to publish</param>
        void PublishHistoric(List<INavigable> historic);

        /// <summary>
        /// Register HistoricObserver as WeakReference.
        /// </summary>
        /// <param name="observer">The HistoricObserver.</param>
        /// <returns>The WeakReference to the HistoricObserver.</returns>
        WeakReference<IHistoricObserver> RegisterObserver(IHistoricObserver observer);

        /// <summary>
        /// Unregister an HistoricObserver.
        /// </summary>
        /// <param name="observer">The HistoricObserver to unregister.</param>
        void UnregisterObserver(IHistoricObserver observer);

        /// <summary>
        /// Notify HistoricObservers of an update on historic.
        /// </summary>
        /// <param name="historic">The updated historic</param>
        void NotifyHistoricObservers(List<INavigable> historic);
    }
}