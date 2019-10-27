using IC.Navigation.Interfaces;
using System;

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