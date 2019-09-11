﻿using IC.Navigation.Extensions.Appium.WindowsDriver.Enums;
using IC.Navigation.Extensions.Appium.WindowsDriver.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IC.Navigation.Extensions.Appium.WindowsDriver
{
    public static class WindowsDriverEx
    {
        /// <summary>
        /// Get the first WindowsElement matching the SearchParam.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c>.</returns>
        public static WindowsElement Get(this WindowsDriver<WindowsElement> windowsDriver, IWDSearchParam searchParam)
        {
            return FindWindowsElement(windowsDriver, searchParam);
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam
        /// and when the condition is met.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c>.</returns>
        public static WindowsElement GetWhen(
            this WindowsDriver<WindowsElement> windowsDriver,
            IWDSearchParam searchParam,
            string attributeName,
            string expectedAttributeValue)
        {
            WindowsElement elmt = FindWindowsElement(windowsDriver, searchParam);
            if (elmt == null) return null;
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                WaitForConditionsToBeMet(elmt, expected, cts);
            }

            return elmt;
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c></returns>
        public static WindowsElement GetWhen(
            this WindowsDriver<WindowsElement> windowsDriver,
            IWDSearchParam searchParam,
            Dictionary<string, string> expectedAttribsNamesValues)
        {
            WindowsElement elmt = FindWindowsElement(windowsDriver, searchParam);
            if (elmt == null) return null;
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                WaitForConditionsToBeMet(elmt, expectedAttribsNamesValues, cts);
            }

            return elmt;
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c></returns>
        public static WindowsElement GetWhen(
           this WindowsDriver<WindowsElement> windowsDriver,
           IWDSearchParam searchParam,
           params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues)
        {
            WindowsElement elmt = FindWindowsElement(windowsDriver, searchParam);
            if (elmt == null) return null;
            var expectedDic = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                WaitForConditionsToBeMet(elmt, expectedDic, cts);
            }

            return elmt;
        }

        private static void WaitForConditionsToBeMet(
            WindowsElement elmt,
            Dictionary<string, string> expected,
            CancellationTokenSource cts)
        {
            do
            {
                var actual = GetAttributesValues(elmt, expected.Keys);
                if (AreConditionsMet(expected, actual))
                {
                    break;
                }
            } while (!cts.IsCancellationRequested);
        }

        private static bool AreConditionsMet(
            Dictionary<string, string> first,
            Dictionary<string, string> second)
        {
            return first.Count == second.Count && !first.Except(second).Any();
        }

        private static Dictionary<string, string> GetAttributesValues(WindowsElement elmt, IEnumerable<string> expectedAttribsNames)
        {
            Dictionary<string, string> attributesValues = new Dictionary<string, string>();
            foreach (var attribName in expectedAttribsNames)
            {
                var value = elmt.GetAttribute(attribName);
                attributesValues.Add(attribName, value);
            }

            return attributesValues;
        }

        private static WindowsElement FindWindowsElement(WindowsDriver<WindowsElement> windowsDriver, IWDSearchParam searchParam)
        {
            switch (Enum.Parse(typeof(WDLocators), searchParam.Locator))
            {
                case WDLocators.AutomationId:
                    return windowsDriver.FindElementsByAccessibilityId(searchParam.Value).FirstOrDefault();

                case Enums.WDLocators.ClassName:
                    return windowsDriver.FindElementsByClassName(searchParam.Value).FirstOrDefault();

                case Enums.WDLocators.Name:
                    return windowsDriver.FindElementsByName(searchParam.Value).FirstOrDefault();

                default:
                    throw new Exception($"Unknown locator: {searchParam.Locator}.");
            }
        }
    }
}