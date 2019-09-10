using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.Extensions.Appium.WindowsDriver.Interfaces;
using IC.Navigation.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface IFacade : INavigatorSession, INavigables, IWindowsDriverSession
    {
    }
}