using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Tests.App.UIAccessibility.Appium.Interfaces
{
    public interface IFacade : INavigatorSession, INavigables, IWindowsDriverSession
    {
        /// <summary>
        /// Get INavigable by their attribute UIArtifact.UsageName.
        /// </summary>
        /// <param name="usageName">The expected usage name.</param>
        /// <returns>The matching INavigable, otherwise <c>null</c>.</returns>
        INavigable GetINavigableByUsageName(string usageName);
    }
}