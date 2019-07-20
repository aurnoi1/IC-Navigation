using IC.Navigation.Interfaces;
using Moq;
using System;
using System.Collections.Generic;

namespace IC.Navigation.UnitTests.Collections
{
    public class NavigablesFixture
    {
        public NavigablesFixture()
        {
            Mock<Action> action = new Mock<Action>();
            Mock<INavigable> n1 = new Mock<INavigable>();
            Mock<INavigable> n2 = new Mock<INavigable>();
            Mock<INavigable> n3 = new Mock<INavigable>();
            Mock<INavigable> n4 = new Mock<INavigable>();
            Mock<INavigable> n5 = new Mock<INavigable>();

            n1.Setup(x => x.PublishExistsStatus()).Returns(true);
            n1.Setup(x => x.GetActionToNext())
                .Returns(new Dictionary<INavigable, Action>()
                {
                    { n2.Object, action.Object },
                    { n3.Object, action.Object },
                    { n4.Object, action.Object },
                });

            n2.Setup(x => x.GetActionToNext())
                .Returns(new Dictionary<INavigable, Action>()
                {
                    { n1.Object, action.Object },
                    { n3.Object, action.Object }
                });

            n3.Setup(x => x.GetActionToNext())
                .Returns(new Dictionary<INavigable, Action>()
                {
                    { n1.Object, action.Object },
                    { n2.Object, action.Object }
                });

            n4.Setup(x => x.GetActionToNext())
                .Returns(new Dictionary<INavigable, Action>()
                {
                    { n3.Object, action.Object },
                });

            // No connection to other nodes.
            n5.Setup(x => x.GetActionToNext())
                .Returns(new Dictionary<INavigable, Action>()
                {
                });

            Nodes = new HashSet<INavigable>() { n1.Object, n2.Object, n3.Object, n4.Object };
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