
using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Exceptions
{
    public class PathNotFound : Exception
    {
        public PathNotFound(INavigable origin, INavigable destination) 
            : base($"No path was found between \"{origin.GetType().FullName}\" and \"{destination.GetType().FullName}\".")
        {
        }
    }
}
