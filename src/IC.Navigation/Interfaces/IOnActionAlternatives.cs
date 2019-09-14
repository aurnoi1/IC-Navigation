using System;
using System.Collections.Generic;
using System.Threading;

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
        /// The alternative action.
        /// </summary>
        Action<CancellationToken> AlternativateAction { get; }
    }
}