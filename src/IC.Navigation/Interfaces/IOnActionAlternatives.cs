using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Alternative Navigables that are possible after an action.
    /// </summary>
    public interface IOnActionAlternatives
    {
        /// <summary>
        /// The possible Navigables.
        /// </summary>
        List<INavigable> Navigables { get; }

        /// <summary>
        /// The alternative action.
        /// </summary>
        Action<CancellationToken> AlternativateAction { get; }
    }
}