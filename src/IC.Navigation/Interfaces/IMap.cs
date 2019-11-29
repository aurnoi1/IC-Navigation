using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IC.Navigation.Interfaces
{
    public interface IMap
    {
        /// <summary>
        /// The nodes of INavigables forming the Graph.
        /// </summary>
        public HashSet<INavigable> Nodes { get; }

        public IGraph Graph { get; }

        public ILog Log {get;}

        public CancellationToken GlobalCancellationToken { get; }
    }
}
