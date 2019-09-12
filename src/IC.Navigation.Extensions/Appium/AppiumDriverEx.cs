﻿using IC.Navigation.Extensions.Appium.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IC.Navigation.Extensions.Appium
{
    public static class AppiumDriverEx
    {
        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver"></param>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public static T Get<T>(this AppiumDriver<T> driver, ISearchParam searchParam) where T : IWebElement
        {
            return FindFirstElement(driver, searchParam);
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public static T Get<T>(
            this AppiumDriver<T> driver,
            ISearchParam searchParam,
            TimeSpan timeout) where T : IWebElement
        {
            using (var cts = new CancellationTokenSource(timeout))
            {
                return Get(driver, searchParam, cts.Token);
            }
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="ct">The CancellationToken used to stop to wait for the control to be found.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public static T Get<T>(
            this AppiumDriver<T> driver,
            ISearchParam searchParam,
            CancellationToken ct) where T : IWebElement
        {
            using (var cts = new CancellationTokenSource())
            {
                while (!ct.IsCancellationRequested)
                {
                    var match = FindFirstElement(driver, searchParam);
                    if (match != null) return match;
                }
            }

            return default;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam
        /// and when the condition is met.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public static T GetWhen<T>(
            this AppiumDriver<T> driver,
            ISearchParam searchParam,
            TimeSpan timeout,
            string attributeName,
            string expectedAttributeValue) where T : IWebElement
        {
            T elmt = default;
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            using (CancellationTokenSource cts = new CancellationTokenSource(timeout))
            {
                elmt = GetWhen(driver, searchParam, cts.Token, expected);
            }

            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="ct">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public static T GetWhen<T>(
            this AppiumDriver<T> driver,
            ISearchParam searchParam,
            CancellationToken ct,
            string attributeName,
            string expectedAttributeValue) where T : IWebElement
        {
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            T elmt = GetWhen(driver, searchParam, ct, expected);
            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public static T GetWhen<T>(
            this AppiumDriver<T> driver,
            ISearchParam searchParam,
            TimeSpan timeout,
            Dictionary<string, string> expectedAttribsNamesValues) where T : IWebElement
        {
            T elmt = default;
            using (CancellationTokenSource cts = new CancellationTokenSource(timeout))
            {
                elmt = GetWhen(driver, searchParam, cts.Token, expectedAttribsNamesValues);
            }

            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public static T GetWhen<T>(
           this AppiumDriver<T> driver,
           ISearchParam searchParam,
           TimeSpan timeout,
           params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues) where T : IWebElement
        {
            T elmt = default;
            var expectedDic = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            using (CancellationTokenSource cts = new CancellationTokenSource(timeout))
            {
                elmt = GetWhen(driver, searchParam, cts.Token, expectedDic);
            }

            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="ct">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public static T GetWhen<T>(
           this AppiumDriver<T> driver,
           ISearchParam searchParam,
           CancellationToken ct,
           params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues) where T : IWebElement
        {
            var expectedDic = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            T elmt = GetWhen(driver, searchParam, ct, expectedDic);
            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="ct">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public static T GetWhen<T>(
           this AppiumDriver<T> driver,
           ISearchParam searchParam,
           CancellationToken ct,
           Dictionary<string, string> expectedAttribsNamesValues) where T : IWebElement
        {
            T elmt = FindFirstElement(driver, searchParam);
            if (elmt == null) return default;
            if (!WaitForConditionsToBeMet(elmt, expectedAttribsNamesValues, ct))
                return default;

            return elmt;
        }

        #region Private

        private static bool WaitForConditionsToBeMet(
            IWebElement elmt,
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
            IWebElement elmt,
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

        private static T FindFirstElement<T>(AppiumDriver<T> driver, ISearchParam searchParam) where T : IWebElement
        {
            return driver.FindElements(searchParam.Locator, searchParam.Value).FirstOrDefault();
        }

        #endregion Private
    }
}