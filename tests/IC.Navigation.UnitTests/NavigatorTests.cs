using Autofac.Extras.Moq;
using IC.Navigation.Interfaces;
using IC.Navigation.UnitTests.Collections;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IC.Navigation.UnitTests
{
    public class NavigatorTests
    {
        [Theory]
        [ClassData(typeof(StraightPathData))]
        public void WhenStraightPathsGraphShouldGetShortestPath(HashSet<INavigable> nodes, Mock<INavigable> origin, Mock<INavigable> destination, List<Mock<INavigable>> expected)
        {
            using (var mock = AutoMock.GetLoose())
            {
                var expectedToList = expected.Select(x => x.Object).ToList();
                Mock<IGraph> iGraph = mock.Mock<IGraph>();
                iGraph.Setup(g => g.GetShortestPath(origin.Object, destination.Object)).Returns(expectedToList);
                var iut = mock.Mock<Navigator>(); // Navigator is abstract so it need to be mocked.
                iut.SetupGet(x => x.Graph).Returns(iGraph.Object); // Set mockedNavigator.Graph
                iut.CallBase = true; // Ensure to call default implementation of virtual members.
                var actual = iut.Object.GetShortestPath(origin.Object, destination.Object);
                Assert.Equal(expectedToList, actual);
                iGraph.Verify(x => x.GetShortestPath(origin.Object, destination.Object), Times.Exactly(1));
            }
        }

        [Theory]
        [ClassData(typeof(StraightPathData))]
        public void GoToShouldResolvePathToDestination(HashSet<INavigable> nodes, Mock<INavigable> origin, Mock<INavigable> destination, List<Mock<INavigable>> expected)
        {
            using (var mock = AutoMock.GetLoose())
            {
                var expectedToList = expected.Select(x => x.Object).ToList();
                Mock<IGraph> iGraph = mock.Mock<IGraph>();
                iGraph.Setup(g => g.GetShortestPath(origin.Object, destination.Object)).Returns(expectedToList);
                var iut = mock.Mock<Navigator>();
                iut.SetupGet(x => x.Graph).Returns(iGraph.Object);
                iut.CallBase = true;
                Mock<ISession> session = mock.Mock<ISession>();
                foreach (var node in expected)
                {
                    node.SetupGet(n => n.Session).Returns(session.Object);
                    node.Setup(n => n.WaitForExists()).Returns(true);
                }

                var actual = iut.Object.GoTo(origin.Object, destination.Object);
                Assert.Equal(expected.Last().Object, actual);
                iGraph.Verify(x => x.GetShortestPath(origin.Object, destination.Object), Times.Exactly(1));
            }
        }
    }
}