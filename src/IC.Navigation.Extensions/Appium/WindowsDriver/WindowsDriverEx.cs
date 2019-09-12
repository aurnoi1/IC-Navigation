using IC.Navigation.Extensions.Appium.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
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
        public static WindowsElement Get(this WindowsDriver<WindowsElement> windowsDriver, ISearchParam searchParam)
        {
            return FindFirstElement(windowsDriver, searchParam);
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c>.</returns>
        public static WindowsElement Get(
            this WindowsDriver<WindowsElement> windowsDriver, 
            ISearchParam searchParam, 
            TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource(timeout))
            {
                return Get(windowsDriver, searchParam, cts.Token);
            }
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="ct">The CancellationToken used to stop to wait for the control to be found.</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c>.</returns>
        public static WindowsElement Get(
            this WindowsDriver<WindowsElement> windowsDriver,
            ISearchParam searchParam,
            CancellationToken ct)
        {
            using (var cts = new CancellationTokenSource())
            {
                while (!ct.IsCancellationRequested)
                {
                    var match = FindFirstElement(windowsDriver, searchParam);
                    if (match != null) return match;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam
        /// and when the condition is met.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c>.</returns>
        public static WindowsElement GetWhen(
            this WindowsDriver<WindowsElement> windowsDriver,
            ISearchParam searchParam,
            TimeSpan timeout,
            string attributeName,
            string expectedAttributeValue)
        {
            WindowsElement elmt = default;
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            using (CancellationTokenSource cts = new CancellationTokenSource(timeout))
            {
                elmt = GetWhen(windowsDriver, searchParam, cts.Token, expected);
            }

            return elmt;
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="ct">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c>.</returns>
        public static WindowsElement GetWhen(
            this WindowsDriver<WindowsElement> windowsDriver,
            ISearchParam searchParam,
            CancellationToken ct,
            string attributeName,
            string expectedAttributeValue)
        {
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            WindowsElement elmt = GetWhen(windowsDriver, searchParam, ct, expected);
            return elmt;
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c></returns>
        public static WindowsElement GetWhen(
            this WindowsDriver<WindowsElement> windowsDriver,
            ISearchParam searchParam,
            TimeSpan timeout,
            Dictionary<string, string> expectedAttribsNamesValues)
        {
            WindowsElement elmt = default;
            using (CancellationTokenSource cts = new CancellationTokenSource(timeout))
            {
                elmt = GetWhen(windowsDriver, searchParam, cts.Token, expectedAttribsNamesValues);
            }

            return elmt;
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c></returns>
        public static WindowsElement GetWhen(
           this WindowsDriver<WindowsElement> windowsDriver,
           ISearchParam searchParam,
           TimeSpan timeout,
           params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues)
        {
            WindowsElement elmt = default;
            var expectedDic = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            using (CancellationTokenSource cts = new CancellationTokenSource(timeout))
            {
                elmt = GetWhen(windowsDriver, searchParam, cts.Token, expectedDic);
            }

            return elmt;
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="ct">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c></returns>
        public static WindowsElement GetWhen(
           this WindowsDriver<WindowsElement> windowsDriver,
           ISearchParam searchParam,
           CancellationToken ct,
           params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues)
        {
            var expectedDic = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            WindowsElement elmt = GetWhen(windowsDriver, searchParam, ct, expectedDic);
            return elmt;
        }

        /// <summary>
        /// Get the first WindowsElement matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <param name="ct">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WindowsElement, otherwise <c>null</c></returns>
        public static WindowsElement GetWhen(
           this WindowsDriver<WindowsElement> windowsDriver,
           ISearchParam searchParam,
           CancellationToken ct,
           Dictionary<string, string> expectedAttribsNamesValues
           )
        {
            WindowsElement elmt = FindFirstElement(windowsDriver, searchParam);
            if (elmt == null) return null;
            if (!WaitForConditionsToBeMet(elmt, expectedAttribsNamesValues, ct))
                return null;

            return elmt;
        }

        #region Private

        private static bool WaitForConditionsToBeMet(
            WindowsElement elmt,
            Dictionary<string, string> expected,
            CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var actual = GetAttributesValues(elmt, expected.Keys, ct);
                if (AreConditionsMet(expected, actual))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AreConditionsMet(
            Dictionary<string, string> first,
            Dictionary<string, string> second)
        {
            return first.Count == second.Count && !first.Except(second).Any();
        }

        private static Dictionary<string, string> GetAttributesValues(
            WindowsElement elmt,
            IEnumerable<string> expectedAttribsNames,
            CancellationToken ct)
        {
            Dictionary<string, string> attributesValues = new Dictionary<string, string>();
            foreach (var attribName in expectedAttribsNames)
            {
                if (ct.IsCancellationRequested) return null;
                var value = elmt.GetAttribute(attribName);
                attributesValues.Add(attribName, value);
            }

            return attributesValues;
        }

        private static T FindFirstElement<T>(AppiumDriver<T> windowsDriver, ISearchParam searchParam) where T : IWebElement
        {
            return windowsDriver.FindElements(searchParam.Locator, searchParam.Value).FirstOrDefault();
        }

        #endregion Private
    }
}