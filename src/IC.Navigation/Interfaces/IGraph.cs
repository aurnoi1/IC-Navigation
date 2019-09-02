using System.Collections.Generic;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines a Graph.
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// Get the shortest path from an origin to a destination.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The List of INavigable from the origin to the destination.</returns>
        List<INavigable> GetShortestPath(INavigable origin, INavigable destination);

        /// <summary>
        /// The nodes of INavigables forming the Graph.
        /// </summary>
        HashSet<INavigable> Nodes { get; }
    }
}