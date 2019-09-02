﻿using OpenQA.Selenium.Appium.Windows;
using System;
using System.Diagnostics;
using System.Linq;

namespace IC.Navigation.Extensions.Appium
{
    public static class WindowsDriverEx
    {
        /// <summary>
        /// Wait for a WindowsElement to be found using the specified search method.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver.</param>
        /// <param name="searchMethod">The method performs to find the WindowsElement.</param>
        /// <param name="timeout">The maximum amount of time to wait.</param>
        /// <returns>The WindowsElement if found, otherwise <c>null</c>.</returns>
        public static WindowsElement FindElement(this WindowsDriver<WindowsElement> windowsDriver, Func<WindowsElement> searchMethod, TimeSpan timeout)
        {
            Stopwatch timer = Stopwatch.StartNew();
            do
            {
                var match = searchMethod();
                if (match != null)
                {
                    timer.Stop();
                    return match;
                }
            } while (timer.Elapsed < timeout);

            return null;
        }

        /// <summary>
        /// Wait for a WindowsElement to be found by AccessibilityId.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver.</param>
        /// <param name="accessibilityId">The AccessibilityId of the expected WindowsElement.</param>
        /// <param name="timeout">The maximum amount of time to wait.</param>
        /// <returns>The first WindowsElement matching the AccessibilityId, otherwise <c>null</c>.</returns>
        public static WindowsElement FindElementByAccessibilityId(this WindowsDriver<WindowsElement> windowsDriver, string accessibilityId, TimeSpan timeout)
        {
            return FindElementByAccessibilityId(windowsDriver, accessibilityId, timeout, 0);
        }

        /// <summary>
        /// Wait for a WindowsElement to be found by AccessibilityId.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver.</param>
        /// <param name="accessibilityId">The AccessibilityId of the WindowsElement.</param>
        /// <param name="timeout">The maximum amount of time to wait.</param>
        /// <param name="index">The expected index among the WindowsElements matching the AccessibilityId.</param>
        /// <returns>The WindowsElement matching the AccessibilityId, otherwise <c>null</c>.</returns>
        public static WindowsElement FindElementByAccessibilityId(this WindowsDriver<WindowsElement> windowsDriver, string accessibilityId, TimeSpan timeout, int index)
        {
            Func<WindowsElement> searchMethod = () => windowsDriver.FindElementsByAccessibilityId(accessibilityId).ToList().ElementAtOrDefault(index);
            return FindElement(windowsDriver, searchMethod, timeout);
        }
    }
}