using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using IC.Navigation.Interfaces;
using IC.Navigation.UnitTests.Collections;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IC.Navigation.UnitTests
{
    public class NavigatorSessionTests
    {
#pragma warning disable xUnit1026

        [Theory]
        [ClassData(typeof(StraightPathData))]
        public void Should_Get_ShortestPath(HashSet<INavigable> _, Mock<INavigable> origin, Mock<INavigable> destination, List<Mock<INavigable>> expected)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expectedToList = expected.Select(x => x.Object).ToList();
            Mock<IGraph> iGraph = new Mock<IGraph>();
            iGraph.Setup(g => g.GetShortestPath(origin.Object, destination.Object)).Returns(expectedToList);
            var sut = new Mock<NavigatorSession>(); // Navigator is abstract so it need to be mocked.
            sut.SetupGet(x => x.Graph).Returns(iGraph.Object); // Set mockedNavigator.Graph
            sut.CallBase = true; // Ensure to call default implementation of virtual members.

            var actual = sut.Object.GetShortestPath(origin.Object, destination.Object);

            Assert.Equal(expectedToList, actual);
            iGraph.Verify(x => x.GetShortestPath(origin.Object, destination.Object), Times.Exactly(1));
        }

#pragma warning disable xUnit1026

        [Theory]
        [ClassData(typeof(StraightPathData))]
        public void Should_Resolve_Path_To_Destination(HashSet<INavigable> _, Mock<INavigable> origin, Mock<INavigable> destination, List<Mock<INavigable>> expected)
        {
            var expectedToList = expected.Select(x => x.Object).ToList();
            Mock<IGraph> iGraph = new Mock<IGraph>();
            iGraph.Setup(g => g.GetShortestPath(origin.Object, destination.Object)).Returns(expectedToList);
            var sut = new Mock<NavigatorSession>();
            sut.SetupGet(x => x.Graph).Returns(iGraph.Object);
            sut.CallBase = true;
            Mock<ISession> session = new Mock<ISession>();
            foreach (var node in expected)
            {
                node.SetupGet(n => n.Session).Returns(session.Object);
                node.Setup(n => n.PublishStatus().Exists).Returns(true);
            }

            var actual = sut.Object.GoTo(origin.Object, destination.Object);
            Assert.Equal(expected.Last().Object, actual);
            iGraph.Verify(x => x.GetShortestPath(origin.Object, destination.Object), Times.Exactly(1));
        }
    }
}