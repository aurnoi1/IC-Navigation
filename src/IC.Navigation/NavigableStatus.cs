using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation
{
    public class NavigableStatus : INavigableStatus
    {
        /// <summary>
        /// The Exists status.
        /// </summary>
        public bool Exists { get; set; }
    }
}
