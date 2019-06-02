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
                INavigator iut = mock.Create<Navigator>();
                var actual = iut.GetShortestPath(origin.Object, destination.Object);
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
                foreach (var node in expected)
                {
                    node.Setup(n => n.WaitForExists()).Returns(true);
                }

                INavigator iut = mock.Create<Navigator>();
                var actual = iut.GoTo(origin.Object, destination.Object);
                Assert.Equal(expected.Last().Object, actual);
                iGraph.Verify(x => x.GetShortestPath(origin.Object, destination.Object), Times.Exactly(1));
            }
        }

        [Theory]
        [ClassData(typeof(StraightPathData))]
        public void HistoricShouldMatchTheCrossedNode(HashSet<INavigable> nodes, Mock<INavigable> origin, Mock<INavigable> destination, List<Mock<INavigable>> expected)
        {
            using (var mock = AutoMock.GetLoose())
            {
                var expectedToList = expected.Select(x => x.Object).ToList();
                var iGraph = mock.Mock<IGraph>();
                iGraph.Setup(g => g.GetShortestPath(origin.Object, destination.Object))
                    .Returns(expectedToList);

                var iut = mock.Create<Navigator>();

                // Arrange nodes to react as real implementation, which call Navigator methods.
                for (int i = 0; i < expected.Count; i++)
                {
                    var node = expected[i];
                    node.Setup(n => n.WaitForExists())
                        .Callback(() => iut.SetLast(node.Object, true))
                        .Returns(true);

                    node.Setup(n => n.CompareTypeName(node.Object))
                        .Returns(true);

                    if (expected.Count == i + 1) break;
                    var nextNode = expected[i + 1].Object;
                    node.Setup(x => x.StepToNext(nextNode))
                        .Callback(() => iut.StepToNext(node.Object.GetActionToNext(), nextNode));
                }

                var actual = iut.GoTo(origin.Object, destination.Object);
                Assert.Equal(expectedToList, iut.Historic);
            }
        }
    }
}