using IC.Navigation.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Navigation.Extensions.Appium.WindowsDriver.Interfaces
{
    public interface IWindowsDriverSession : ISession, IDisposable
    {
        /// <summary>
        /// The WindowsDriver.
        /// </summary>
        WindowsDriver<WindowsElement> WindowsDriver { get; }
    }
}