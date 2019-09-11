using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Interfaces
{
    public interface INavigableStatus
    {
        /// <summary>
        /// The Exists status of the Navigable.
        /// </summary>
        bool Exists { get; }
    }
}
