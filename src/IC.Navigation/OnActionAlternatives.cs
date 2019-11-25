using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Navigation
{
    /// <summary>
    /// Alternative INavigables that are possible after an action.
    /// </summary>
    public class OnActionAlternatives : IOnActionAlternatives
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Navigator"/> class.
        /// </summary>
        /// <param name="alternativeAction">The alternative action.</param>
        /// <param name="iNavigables">The possible INavigables.</param>
        public OnActionAlternatives(Action<CancellationToken> alternativeAction, List<INavigable> iNavigables)
        {
            AlternativateAction = alternativeAction;
            Navigables = iNavigables;
        }

        /// <summary>
        /// The possible INavigables.
        /// </summary>
        public List<INavigable> Navigables { get; private set; }

        /// <summary>
        /// The alternative action.
        /// </summary>
        public Action<CancellationToken> AlternativateAction { get; private set; }
    }
}