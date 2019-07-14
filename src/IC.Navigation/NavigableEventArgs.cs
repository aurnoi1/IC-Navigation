using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.Navigation
{
    /// <summary>
    /// Event data of INavigable. 
    /// </summary>
    public class NavigableEventArgs : EventArgs, INavigableEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigableEventArgs"/> class.
        /// </summary>
        public NavigableEventArgs()
        {
        }

        /// <summary>
        /// <c>true</c> if the INavigable exists, otherwise <c>false</c>.
        /// </summary>
        public bool Exists { get; set; }
    }
}
