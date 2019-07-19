using IC.Navigation.Interfaces;
using IC.Navigation.UnitTests.Collections;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace IC.Navigation.UnitTests
{
    public class GraphTests
    {
        public GraphTests()
        {
        }

        [Theory]
        [ClassData(typeof(StraightPathData))]
        public void WhenStraightPathsGraphShouldGetShortestPath(HashSet<INavigable> nodes, Mock<INavigable> origin, Mock<INavigable> destination, List<Mock<INavigable>> expected)
        {
            IGraph iut = new Graph(nodes);
            var actual = iut.GetShortestPath(origin.Object, destination.Object);
            Assert.Equal(expected.Select(x => x.Object).ToList(), actual);
        }

        [Theory]
        [ClassData(typeof(NoPathData))]
        public void WhenNoPathGraphShouldNoPath(HashSet<INavigable> nodes, INavigable origin, INavigable destination, List<INavigable> expected)
        {
            IGraph iut = new Graph(nodes);
            var actual = iut.GetShortestPath(origin, destination);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(OriginIsDestinationData))]
        public void WhenOriIsDestinationGraphShouldGetNoPath(HashSet<INavigable> nodes, INavigable origin, INavigable destination, List<INavigable> expected)
        {
            IGraph iut = new Graph(nodes);
            var actual = iut.GetShortestPath(origin, destination);
            Assert.Equal(expected, actual);
        }

    }
}