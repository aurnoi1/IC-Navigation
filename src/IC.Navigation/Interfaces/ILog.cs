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
        /// Event raised when the last known existing INavigable has changed.
        /// </summary>
        event EventHandler<INavigableEventArgs> HistoricChanged;
    }
}