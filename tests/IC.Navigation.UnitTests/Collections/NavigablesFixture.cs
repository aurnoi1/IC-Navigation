using IC.Navigation.Enums;
using IC.Navigation.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Navigation.UnitTests.Collections
{
    public class NavigablesFixture
    {
        public NavigablesFixture()
        {
            Mock<Action<CancellationToken>> action = new Mock<Action<CancellationToken>>();
            Mock<INavigable> n1 = new Mock<INavigable>();
            Mock<INavigable> n2 = new Mock<INavigable>();
            Mock<INavigable> n3 = new Mock<INavigable>();
            Mock<INavigable> n4 = new Mock<INavigable>();
            Mock<INavigable> n5 = new Mock<INavigable>();
            Mock<HashSet<INavigable>> nodes = new Mock<HashSet<INavigable>>();

            n1.Setup(x => x.PublishStatus().Exist).Returns(new State<bool>(StatesNames.Exist, true));
            n1.Setup(x => x.GetActionToNext())                             
                .Returns(new Dictionary<INavigable, Action<CancellationToken>>()
                {
                    { n2.Object, action.Object },
                    { n3.Object, action.Object },
                    { n4.Object, action.Object },
                });

            n2.Setup(x => x.GetActionToNext())
                .Returns(new Dictionary<INavigable, Action<CancellationToken>>()
                {
                    { n1.Object, action.Object },
                    { n3.Object, action.Object }
                });

            n3.Setup(x => x.GetActionToNext())
                .Returns(new Dictionary<INavigable, Action<CancellationToken>>()
                {
                    { n1.Object, action.Object },
                    { n2.Object, action.Object }
                });

            n4.Setup(x => x.GetActionToNext())
                .Returns(new Dictionary<INavigable, Action<CancellationToken>>()
                {
                    { n3.Object, action.Object },
                });

            // No connection to other nodes.
            n5.Setup(x => x.GetActionToNext())
                .Returns(new Dictionary<INavigable, Action<CancellationToken>>()
                {
                });

            Nodes = nodes.Object;
            Nodes.Add(n1.Object);
            Nodes.Add(n2.Object);
            Nodes.Add(n3.Object);
            Nodes.Add(n4.Object);
            Nodes.Add(n5.Object);

            N1 = n1;
            N2 = n2;
            N3 = n3;
            N4 = n4;
            N5 = n5;
        }

        public HashSet<INavigable> Nodes { get; private set; }
        public Mock<INavigable> N1;
        public Mock<INavigable> N2;
        public Mock<INavigable> N3;
        public Mock<INavigable> N4;
        public Mock<INavigable> N5;
    }
}