﻿using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IC.Navigation
{
    /// <summary>
    /// Defines a Graph.
    /// </summary>
    public class Graph : IGraph
    {
        #region Fields

        private int edgeCounter;

        #endregion Fields

        private Graph()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="Navigables">The Navigables to add to this Graph.</param>
        public Graph(HashSet<INavigable> Navigables)
        {
            Nodes = Navigables;
            AddNodesReferences();
        }

        #region Propeties

        #region Private

        /// <summary>
        /// An automatically increment number of Edge.
        /// </summary>
        private int EdgeCounter
        {
            get
            {
                if (Vertices.Count == 0)
                {
                    edgeCounter = 1;
                }

                return edgeCounter++;
            }

            set
            {
                edgeCounter = value;
            }
        }

        /// <summary>
        /// The Vertices composing the Graph.
        /// </summary>
        private Dictionary<string, Dictionary<string, int>> Vertices = new Dictionary<string, Dictionary<string, int>>();

        private List<List<string>> AlreadyFoundPaths = new List<List<string>>();

        #endregion Private

        #region Internal

        /// <summary>
        /// The nodes of INavigable forming the Graph.
        /// </summary>
        public HashSet<INavigable> Nodes { get; private set; }

        #endregion Internal

        #endregion Propeties

        #region Methods

        #region Public

        /// <summary>
        /// Add Nodes references to the Graph.
        /// </summary>
        private void AddNodesReferences()
        {
            foreach (var node in Nodes)
            {
                var destinations = node.GetActionToNext().Select(x => x.Key).ToList();
                if (destinations.Count == 0)
                {
                    AddVertex(node.ToString(), node.ToString());
                }
                else
                {
                    var destinantionsNames = destinations.Select(x => x.ToString()).ToList();
                    AddVertices(node.ToString(), destinantionsNames);
                }
            }
        }

        /// <summary>
        /// Add a Vertex to the Graph's Vertices list.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        private void AddVertex(string origin, string destination)
        {
            Dictionary<string, int> edges = new Dictionary<string, int>();
            if (Vertices.ContainsKey(origin))
            {
                // Add value to an existing entry.
                var currentEdges = Vertices[origin];
                currentEdges.Add(destination, EdgeCounter);
                edges = currentEdges;
            }
            else
            {
                edges.Add(destination, EdgeCounter);
            }

            Vertices[origin] = edges;
        }

        /// <summary>
        /// Add Vertices to the Graph's Vertices list.
        /// </summary>
        /// <param name="origin">the origin.</param>
        /// <param name="destinations">The list of destinations.</param>
        private void AddVertices(string origin, List<string> destinations)
        {
            Dictionary<string, int> edges = new Dictionary<string, int>();
            foreach (var destination in destinations)
            {
                AddVertex(origin, destination);
            }
        }

        /// <summary>
        /// Get the shortest path from an origin to a destination.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The List of INavigable from the origin to the destination.</returns>
        public List<INavigable> GetShortestPath(INavigable origin, INavigable destination)
        {
            if (Vertices.Count == 0)
            {
                throw new Exception($"The \"Vertices\" is empty. Add vertex using {nameof(AddVertex)} or {nameof(AddVertices)}" +
                    $" before to call this method.");
            }

            string originName = origin.ToString();
            string destinationName = destination.ToString();
            List<INavigable> navigables = new List<INavigable>();

            // Returns path already known
            var pathAlreadyFound = GetKnownPath(originName, destinationName);
            if (pathAlreadyFound != null)
            {
                return pathAlreadyFound;
            }

            var previous = new Dictionary<string, string>();
            var distances = new Dictionary<string, int>();
            var nodes = new List<string>();
            List<string> path = new List<string>();
            foreach (var vertex in Vertices)
            {
                if (vertex.Key == originName)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);
                var smallest = nodes[0];
                nodes.Remove(smallest);
                if (smallest == destinationName)
                {
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in Vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            // Add the origin in the path.
            if (destinationName != originName & path.Count > 0)
            {
                path.Add(originName);
            }

            path.Reverse();
            AlreadyFoundPaths.Add(path);
            navigables = GetNavigablesByName(path);
            return navigables;
        }

        #endregion Public

        #region Private

        private List<INavigable> GetNavigablesByName(List<string> names)
        {
            List<INavigable> navigables = new List<INavigable>();
            foreach (var name in names)
            {
                navigables.Add(GetNavigableByName(name));
            }

            return navigables;
        }

        private INavigable GetNavigableByName(string name)
        {
            return Nodes.Where(x => x.ToString() == name).Single();
        }

        /// <summary>
        /// Gets known path.
        /// </summary>
        /// <param name="origin">The Origin.</param>
        /// <param name="destination">The Destination.</param>
        /// <returns>The known paths.</returns>
        private List<INavigable> GetKnownPath(string origin, string destination)
        {
            foreach (var path in AlreadyFoundPaths)
            {
                // Path is empty when the INavigable goes nowhere.
                if (path.Count != 0)
                {
                    var pathOrigin = path.First();
                    var pathDestination = path.Last();
                    if (pathOrigin == origin && pathDestination == destination)
                    {
                        return GetNavigablesByName(path);
                    }
                }
            };

            return null;
        }

        #endregion Private

        #endregion Methods
    }
}