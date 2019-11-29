using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Tests.App.Poms.Appium
{
    public class Browser : IBrowser
    {
        public IMap Map { get; }

        public INavigator Navigator { get; }

        public Browser(IMap map)
        {
            Map = map;
        }
    }
}
