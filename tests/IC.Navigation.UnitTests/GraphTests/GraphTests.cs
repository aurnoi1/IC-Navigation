using IC.Navigation.Interfaces;
using IC.Navigation.UnitTests.Collections;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AutoFixture.Xunit2;
using AutoFixture.AutoMoq;
using AutoFixture;

namespace IC.Navigation.UnitTests
{
    public class GraphTests
    {
        public GraphTests()
        {
        }

        [Theory]
        [ClassData(typeof(StraightPathData))]
        public void GetShortestPath_Should_Returns_Shortest_Path_Of_Straight_Paths(HashSet<INavigable> nodes, Mock<INavigable> origin, Mock<INavigable> destination, List<Mock<INavigable>> expected)
        {
            IGraph sut = new Graph(nodes);
            var actual = sut.GetShortestPath(origin.Object, destination.Object);
            Assert.Equal(expected.Select(x => x.Object).ToList(), actual);
        }

        [Theory]
        [ClassData(typeof(NoPathData))]
        public void GetShortestPath_Should_Return_No_Path(HashSet<INavigable> nodes, INavigable origin, INavigable destination, List<INavigable> expected)
        {
            IGraph sut = new Graph(nodes);
            var actual = sut.GetShortestPath(origin, destination);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(OriginIsDestinationData))]
        public void GetShortestPath_Should_Returns_No_Path_When_Origin_Is_Destination(HashSet<INavigable> nodes, INavigable origin, INavigable destination, List<INavigable> expected)
        {
            IGraph sut = new Graph(nodes);
            var actual = sut.GetShortestPath(origin, destination);
            Assert.Equal(expected, actual);
        }
    }
}