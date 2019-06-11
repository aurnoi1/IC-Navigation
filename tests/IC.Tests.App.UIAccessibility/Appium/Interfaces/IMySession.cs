using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Tests.App.UIAccessibility.Appium.Interfaces
{
    public interface IMySession : IMyNavigables, IWindowsDriverSession
    {

        /// <summary>
        /// Multiplicator to adjust the timeout to the environment when waiting for the controls.
        /// </summary>
        uint ThinkTime { get; set; }

        /// <summary>
        /// Adjust the timeout to the environment when waiting for the controls depending the <see cref="ThinkTime"/> value.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The adjusted timeout.</returns>
        TimeSpan AdjustTimeout(TimeSpan timeout);
    }
}