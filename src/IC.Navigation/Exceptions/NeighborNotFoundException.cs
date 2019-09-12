using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Exceptions
{
    public class NeighborNotFoundException : Exception
    {
        public NeighborNotFoundException()
        {
        }

        public NeighborNotFoundException(INavigable neighbor) : base($"Could not find the neighbor \"{neighbor.GetType().FullName}\".")
        {
        }
    }
}
