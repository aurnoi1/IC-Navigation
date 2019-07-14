using System;
using System.Collections.Generic;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines an INavigable.
    /// </summary>
    public interface INavigable : INavigableObservable
    {
        /// <summary>
        /// The navigation session.
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// Waits for the current INavigable to exists.
        /// </summary>
        bool WaitForExists();

        /// <summary>
        /// Gets a Dictionary of action to go to the next INavigable.
        /// </summary>
        /// <returns>A Dictionary of action to go to the next INavigable.</returns>
        Dictionary<INavigable, Action> GetActionToNext();
    }
}