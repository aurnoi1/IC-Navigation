using System;
using System.Collections.Generic;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines a INavigable.
    /// </summary>
    public interface INavigable
    {
        /// <summary>
        /// The navigation session.
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        bool WaitForExists();

        /// <summary>
        /// Gets a Dictionary of action to go to the next INavigable.
        /// </summary>
        /// <returns>A Dictionary of action to go to the next INavigable.</returns>
        Dictionary<INavigable, Action> GetActionToNext();
    }
}