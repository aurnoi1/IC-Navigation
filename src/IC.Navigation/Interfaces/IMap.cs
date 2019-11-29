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
        HashSet<INavigable> Nodes { get; }

        IGraph Graph { get; }

        CancellationToken GlobalCancellationToken { get; }

        HashSet<DynamicPath> DynamicPaths { get; }
    }
}
