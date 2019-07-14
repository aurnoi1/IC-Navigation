using System;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Event data of INavigable.
    /// </summary>
    public interface INavigableEventArgs
    {
        /// <summary>
        /// <c>true</c> if the INavigable exists, otherwise <c>false</c>.
        /// </summary>
        bool Exists { get; set; }
    }
}