using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;

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
        /// <param name="uIAction">The UI action.</param>
        /// <param name="iNavigables">The possible INavigables.</param>
        public OnActionAlternatives(Action uIAction, List<INavigable> iNavigables)
        {
            UIAction = uIAction;
            INavigables = iNavigables;
        }

        /// <summary>
        /// The possible INavigables.
        /// </summary>
        public List<INavigable> INavigables { get; private set; }

        /// <summary>
        /// The UI action.
        /// </summary>
        public Action UIAction { get; private set; }
    }
}