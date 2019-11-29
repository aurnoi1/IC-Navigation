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
        public Nav(IMap map)
        {
            Map = map;
        }

        public override IMap Map { get; set; }
    }
}
