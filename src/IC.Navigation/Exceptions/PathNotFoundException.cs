
using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Exceptions
{
    public class PathNotFoundException : Exception
    {
        public PathNotFoundException(INavigable origin, INavigable destination) 
            : base($"No path was found between \"{origin.GetType().FullName}\" and \"{destination.GetType().FullName}\".")
        {
        }
    }
}
