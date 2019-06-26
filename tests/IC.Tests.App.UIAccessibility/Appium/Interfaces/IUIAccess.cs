using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Tests.App.UIAccessibility.Appium.Interfaces
{
    public interface IUIAccess : INavigables, IWindowsDriverSession
    {
    }
}