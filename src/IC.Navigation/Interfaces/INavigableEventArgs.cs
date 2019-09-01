using System;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Event data of INavigable.
    /// </summary>
    public interface INavigableEventArgs
    {
        /// <summary>
        /// The NavigableStatus.
        /// </summary>
        INavigableStatus NavigableStatus { get; set; }
    }
}