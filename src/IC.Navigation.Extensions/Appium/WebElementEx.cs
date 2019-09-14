﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IC.Navigation.Extensions.Appium
{
    public static class WebElementEx
    {
        private static readonly string timeoutExceptionMsg = "Timeout was reached before attributes match expected values.";

        /// <summary>
        /// Continue operations on this WebElement once the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop waiting for the attributes match their expected values.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>This WebElement once its attributes match the expected values.</returns>
        /// <exception cref="OperationCanceledException">Throw when the task is cancelled.</exception>
        public static T ContinueWhen<T>(
            this T elmt,
            CancellationToken cancellationToken,
            string attributeName,
            string expectedAttributeValue) where T : IWebElement
        {
            if (elmt.WaitUntil(cancellationToken, attributeName, expectedAttributeValue))
                return elmt;

            // WaitUntil() can only return false when the CancellationToken has been Cancelled.
            throw new OperationCanceledException(timeoutExceptionMsg);
        }

        /// <summary>
        /// Continue operations on this WebElement once the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop waiting for the attributes match their expected values.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>This WebElement once its attributes match the expected values.</returns>
        /// <exception cref="OperationCanceledException">Throw when the task is cancelled.</exception>
        public static T ContinueWhen<T>(
            this T elmt,
            CancellationToken cancellationToken,
            params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues) where T : IWebElement
        {
            if (elmt.WaitUntil(cancellationToken, expectedAttribsNamesValues))
                return elmt;

            // WaitUntil() can only return false when the CancellationToken has been Cancelled.
            throw new OperationCanceledException(timeoutExceptionMsg);
        }

        /// <summary>
        /// Continue operations on this WebElement once the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop waiting for the attributes match their expected values.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>This WebElement once its attributes match the expected values.</returns>
        /// <exception cref="OperationCanceledException">Throw when the task is cancelled.</exception>
        public static T ContinueWhen<T>(
            this T elmt,
            CancellationToken cancellationToken,
            Dictionary<string, string> expectedAttribsNamesValues) where T : IWebElement
        {
            if (elmt.WaitUntil(expectedAttribsNamesValues, cancellationToken))
                return elmt;

            // WaitUntil() can only return false when the CancellationToken has been Cancelled.
            throw new OperationCanceledException(timeoutExceptionMsg);
        }

        /// <summary>
        /// Continue operations on this WebElement once the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the attributes to match expected values.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>This WebElement once its attributes match the expected values.</returns>
        /// <exception cref="TimeoutException">Throw when timeout is reached before attributes match expected values.</exception>
        public static T ContinueWhen<T>(
            this T elmt,
            TimeSpan timeout,
            string attributeName,
            string expectedAttributeValue) where T : IWebElement
        {
            if (elmt.WaitUntil(timeout, attributeName, expectedAttributeValue))
                return elmt;

            // WaitUntil() can only return false when the CancellationToken has been Cancelled on timeout.
            throw new TimeoutException(timeoutExceptionMsg);
        }

        /// <summary>
        /// Continue operations on this WebElement once the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the attributes to match expected values.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>This WebElement once its attributes match the expected values.</returns>
        /// <exception cref="TimeoutException">Throw when timeout is reached before attributes match expected values.</exception>
        public static T ContinueWhen<T>(
            this T elmt,
            TimeSpan timeout,
            params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues) where T : IWebElement
        {
            if (elmt.WaitUntil(timeout, expectedAttribsNamesValues))
                return elmt;

            // WaitUntil() can only return false when the CancellationToken has been Cancelled on timeout.
            throw new TimeoutException(timeoutExceptionMsg);
        }

        /// <summary>
        /// Continue operations on this WebElement once the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the attributes to match expected values.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>This WebElement once its attributes match the expected values.</returns>
        /// <exception cref="TimeoutException">Throw when timeout is reached before attributes match expected values.</exception>
        public static T ContinueWhen<T>(
            this T elmt,
            TimeSpan timeout,
            Dictionary<string, string> expectedAttribsNamesValues) where T : IWebElement
        {
            if (elmt.WaitUntil(timeout, expectedAttribsNamesValues))
                return elmt;

            // WaitUntil() can only return false when the CancellationToken has been Cancelled on timeout.
            throw new TimeoutException(timeoutExceptionMsg);
        }

        /// <summary>
        /// Wait until the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the attributes to match expected values.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns><c>true</c> if attributes match their expected values before the end of the timeout, otherwise <c>false</c>.</returns>
        public static bool WaitUntil<T>(
            this T elmt,
            TimeSpan timeout,
            string attributeName,
            string expectedAttributeValue) where T : IWebElement
        {
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            return elmt.WaitUntil(timeout, expected);
        }

        /// <summary>
        /// Wait until the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the attributes to match expected values.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns><c>true</c> if attributes match their expected values before the end of the timeout, otherwise <c>false</c>.</returns>
        public static bool WaitUntil<T>(
            this T elmt,
            TimeSpan timeout,
            params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues) where T : IWebElement
        {
            var expected = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            return elmt.WaitUntil(timeout, expected);
        }

        /// <summary>
        /// Wait until the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="timeout">The maximum amount of time to wait for the attributes to match expected values.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns><c>true</c> if attributes match their expected values before the end of the timeout, otherwise <c>false</c>.</returns>
        public static bool WaitUntil<T>(
            this T elmt,
            TimeSpan timeout,
            Dictionary<string, string> expectedAttribsNamesValues) where T : IWebElement
        {
            using (CancellationTokenSource cts = new CancellationTokenSource(timeout))
            {
                return elmt.WaitUntil(expectedAttribsNamesValues, cts.Token);
            }
        }

        /// <summary>
        /// Wait until the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns><c>true</c> if attributes match their expected values,
        /// otherwise <c>false</c> if the CancellationToken is cancelled.</returns>
        public static bool WaitUntil<T>(
            this T elmt,
            CancellationToken cancellationToken,
            string attributeName,
            string expectedAttributeValue) where T : IWebElement
        {
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            return elmt.WaitUntil(expected, cancellationToken);
        }

        /// <summary>
        /// Wait until the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns><c>true</c> if attributes match their expected values,
        /// otherwise <c>false</c> if the CancellationToken is cancelled.</returns>
        public static bool WaitUntil<T>(
            this T elmt,
            CancellationToken cancellationToken,
            params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues) where T : IWebElement
        {
            var expected = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            return elmt.WaitUntil(expected, cancellationToken);
        }

        /// <summary>
        /// Wait until the attributes match their expected values.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// otherwise <c>false</c> if the CancellationToken is cancelled.</returns>
        public static bool WaitUntil<T>(
            this T elmt,
            Dictionary<string, string> expectedAttribsNamesValues,
            CancellationToken cancellationToken) where T : IWebElement
        {
            if (elmt == null) throw new ArgumentNullException("The WebElement is null.");
            while (!cancellationToken.IsCancellationRequested)
            {
                var actual = GetAttributesValues(elmt, expectedAttribsNamesValues.Keys, cancellationToken);
                if (AreEqual(expectedAttribsNamesValues, actual))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the attributes values of this element.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="elmt">This WebElement.</param>
        /// <param name="attribsNames">The attribute attribute names from where to retrieve the values.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <returns>The attributes names as keys and the expected values.</returns>
        /// <exception cref="ArgumentNullException">Throw when the WebElement is null.</exception>
        public static Dictionary<string, string> GetAttributesValues<T>(
            this T elmt,
            IEnumerable<string> attribsNames,
            CancellationToken cancellationToken) where T : IWebElement
        {
            if (elmt == null) throw new ArgumentNullException("The WebElement is null.");
            Dictionary<string, string> attributesValues = new Dictionary<string, string>();
            foreach (var attribName in attribsNames)
            {
                if (cancellationToken.IsCancellationRequested) return null;
                var value = elmt.GetAttribute(attribName);
                attributesValues.Add(attribName, value);
            }

            return attributesValues;
        }

        #region Private

        private static bool AreEqual(
            Dictionary<string, string> first,
            Dictionary<string, string> second)
        {
            return first.Count == second.Count && !first.Except(second).Any();
        }

        #endregion Private
    }
}