using IC.Navigation;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IC.Tests.App.Poms
{
    public class Nav : Navigator
    {
        private readonly IMap map;
        public Nav(IMap map, CancellationToken globalCancellationToken)
        {
            this.map = map;
            Graph = new Graph(Nodes);
            GlobalCancellationToken = globalCancellationToken;
        }

        public override IGraph Graph { get; }

        public override HashSet<INavigable> Nodes => map.Nodes;

        public override CancellationToken GlobalCancellationToken { get; set; }

        public override void Update<T>(INavigable navigable, IState<T> state)
        {
            // Add a logger here if wanted.
            throw new NotImplementedException();
        }
    }
}
