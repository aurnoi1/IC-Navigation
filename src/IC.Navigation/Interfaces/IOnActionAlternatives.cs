using System;
using System.Collections.Generic;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Alternative INavigables that are possible after an action.
    /// </summary>
    public interface IOnActionAlternatives
    {
        /// <summary>
        /// The possible INavigables.
        /// </summary>
        List<INavigable> INavigables { get; }

        /// <summary>
        /// The UI action.
        /// </summary>
        Action UIAction { get; }
    }
}