using IC.Navigation.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace IC.Navigation.UnitTests.Collections
{
    public class OriginIsDestinationData : INavigablesFixture, IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { Nodes, N1.Object, N1.Object, new List<INavigable>() };
            yield return new object[] { Nodes, N2.Object, N2.Object, new List<INavigable>() };
            yield return new object[] { Nodes, N3.Object, N3.Object, new List<INavigable>() };
            yield return new object[] { Nodes, N4.Object, N4.Object, new List<INavigable>() };
            yield return new object[] { Nodes, N5.Object, N5.Object, new List<INavigable>() };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}