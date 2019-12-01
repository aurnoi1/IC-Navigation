using IC.Navigation.Interfaces;
using System.Collections.Generic;

namespace IC.Navigation
{
    public class DynamicNeighbor
    {
        public INavigable Origin { get; }
        public HashSet<INavigable> Alternatives { get; }

        public DynamicNeighbor(INavigable origin, HashSet<INavigable> alternatives)
        {
            Origin = origin;
            Alternatives = alternatives;
        }
    }
}