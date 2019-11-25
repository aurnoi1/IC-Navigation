using IC.Navigation.Enums;
using IC.Navigation.Exceptions;
using IC.Navigation.Interfaces;
using IC.Navigation.UnitTests.Collections;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace IC.Navigation.UnitTests.NavigatorSessionTests
{
    public class NavigationTests
    {
        public NavigationTests()
        {
        }

#pragma warning disable xUnit1026

        [Theory]
        [ClassData(typeof(StraightPathData))]
        public void GetShortestPath_Should_Returns_ShortestPath(HashSet<INavigable> _, Mock<INavigable> origin, Mock<INavigable> destination, List<Mock<INavigable>> expected)
        {
            var expectedToList = expected.Select(x => x.Object).ToList();
            Mock<IGraph> iGraph = new Mock<IGraph>();
            iGraph.Setup(g => g.GetShortestPath(origin.Object, destination.Object)).Returns(expectedToList);
            var sut = new Mock<Navigator>(); // Navigator is abstract so it need to be mocked.
            sut.SetupGet(x => x.Graph).Returns(iGraph.Object); // Set mockedNavigator.Graph
            sut.CallBase = true; // Ensure to call default implementation of virtual members.

            var actual = sut.Object.GetShortestPath(origin.Object, destination.Object);

            Assert.Equal(expectedToList, actual);
            iGraph.Verify(x => x.GetShortestPath(origin.Object, destination.Object), Times.Exactly(1));
        }

#pragma warning disable xUnit1026

        [Theory]
        [ClassData(typeof(StraightPathData))]
        public void GoTo_Should_Follow_Resolved_Paths(HashSet<INavigable> _, Mock<INavigable> origin, Mock<INavigable> destination, List<Mock<INavigable>> expected)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var expectedToList = expected.Select(x => x.Object).ToList();
            Mock<IGraph> iGraph = new Mock<IGraph>();
            iGraph.Setup(g => g.GetShortestPath(origin.Object, destination.Object)).Returns(expectedToList);
            var sut = new Mock<Navigator>();
            sut.SetupGet(x => x.Graph).Returns(iGraph.Object);
            sut.CallBase = true;
            Mock<INavigator> session = new Mock<INavigator>();
            foreach (var node in expected)
            {
                node.SetupGet(n => n.Navigator).Returns(session.Object);
                node.Setup(n => n.PublishStatus().Exist)
                    .Returns(new State<bool>(node.Object, StatesNames.Exist, true));
                node.Setup(n => n.PublishStatus().Ready)
                    .Returns(new State<bool>(node.Object, StatesNames.Ready, true));
            }

            var actual = sut.Object.GoTo(origin.Object, destination.Object, cts.Token);
            Assert.Equal(expected.Last().Object, actual);
            iGraph.Verify(x => x.GetShortestPath(origin.Object, destination.Object), Times.Exactly(1));
        }
    }
}