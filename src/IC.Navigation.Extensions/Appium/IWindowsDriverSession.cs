using IC.Navigation.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Extensions.Appium
{
    public interface IWindowsDriverSession : ISession, IDisposable
    {
        /// <summary>
        /// The WindowsDriver.
        /// </summary>
        WindowsDriver<WindowsElement> WindowsDriver { get; }
    }
}
