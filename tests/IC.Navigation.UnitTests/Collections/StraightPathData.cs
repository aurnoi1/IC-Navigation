using IC.Navigation.Interfaces;
using Moq;
using System.Collections;
using System.Collections.Generic;

namespace IC.Navigation.UnitTests.Collections
{
    public class StraightPathData : NavigablesFixture, IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { Nodes, N1, N2, new List<Mock<INavigable>>() { N1, N2 } };
            yield return new object[] { Nodes, N1, N3, new List<Mock<INavigable>>() { N1, N3 } };
            yield return new object[] { Nodes, N1, N4, new List<Mock<INavigable>>() { N1, N4 } };
            yield return new object[] { Nodes, N2, N1, new List<Mock<INavigable>>() { N2, N1 } };
            yield return new object[] { Nodes, N2, N3, new List<Mock<INavigable>>() { N2, N3 } };
            yield return new object[] { Nodes, N2, N4, new List<Mock<INavigable>>() { N2, N1, N4 } };
            yield return new object[] { Nodes, N3, N1, new List<Mock<INavigable>>() { N3, N1 } };
            yield return new object[] { Nodes, N3, N2, new List<Mock<INavigable>>() { N3, N2 } };
            yield return new object[] { Nodes, N3, N4, new List<Mock<INavigable>>() { N3, N1, N4 } };
            yield return new object[] { Nodes, N4, N1, new List<Mock<INavigable>>() { N4, N3, N1 } };
            yield return new object[] { Nodes, N4, N3, new List<Mock<INavigable>>() { N4, N3 } };
            yield return new object[] { Nodes, N4, N2, new List<Mock<INavigable>>() { N4, N3, N2 } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}