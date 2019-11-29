using IC.Navigation.Interfaces;
using System.Collections.Generic;

namespace IC.Navigation
{
    public class DynamicPath
    {
        public INavigable Origin { get; }
        public HashSet<INavigable> Alternatives { get; }

        public DynamicPath(INavigable origin, HashSet<INavigable> alternatives)
        {
            Origin = origin;
            Alternatives = alternatives;
        }
    }
}