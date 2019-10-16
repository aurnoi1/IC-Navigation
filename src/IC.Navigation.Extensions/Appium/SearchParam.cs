using IC.Navigation.Extensions.Appium.Interfaces;
using IC.Navigation.Extensions.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IC.Navigation.Extensions.Appium
{
    public class SearchParam<T> : ISearchParam<T> where T : IWebElement
    {
        public SearchParam()
        {
        }

        public SearchParam(
            string locator,
            string value,
            AppiumDriver<T> appiumDriver,
            CancellationToken defaultCancellationToken = default)
        {
            Locator = locator;
            Value = value;
            AppiumDriver = appiumDriver;
            DefaultCancellationToken = defaultCancellationToken;
        }

        public string Locator { get; set; }
        public string Value { get; set; }
        public AppiumDriver<T> AppiumDriver { get; private set; }

        public CancellationToken DefaultCancellationToken { get; private set; }

        #region Find Methods

        /// <summary>
        /// Search the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken used to stop waiting for the control to be found.</param>
        /// <returns>The first matching WebElement.</returns>
        /// <exception cref="OperationCanceledException">Throw when the task is cancelled.</exception>
        public T Find(CancellationToken cancellationToken)
        {
            var elmt = Get(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return elmt;
        }

        /// <summary>
        /// Search the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement.</returns>
        /// <exception cref="TimeoutException">Thrown when timeout is reached before WebElement is found.</exception>
        public T Find(TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);
            var elmt = Get(cts.Token);
            if (cts.Token.IsCancellationRequested)
            {
                throw new TimeoutException("The timeout has been reached.");
            }

            return elmt;
        }

        #endregion
        #region Get Methods


        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public T Get()
        {
            return FindFirstElement();
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public T Get(TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource(timeout))
            {
                return Get(cts.Token);
            }
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens used to stop waiting for the control to be found.
        /// They will be linked to the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public T Get(params CancellationToken[] cancellationTokens)
        {
            var linkedTokens = LinkCancellationTokens(cancellationTokens);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            while (!linkedTokenSource.IsCancellationRequested)
            {
                var match = FindFirstElement();
                if (match != null) return match;
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
        public T GetWhen(
            TimeSpan timeout,
            string attributeName,
            string expectedAttributeValue)
        {
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            return GetWhen(timeout, expected);
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
        public T GetWhen(
           TimeSpan timeout,
           params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues)
        {
            var expected = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            return GetWhen(timeout, expected);
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
        public T GetWhen(
            TimeSpan timeout,
            Dictionary<string, string> expectedAttribsNamesValues)
        {
            T elmt = default;
            using (CancellationTokenSource cts = new CancellationTokenSource(timeout))
            {
                elmt = GetWhen(cts.Token, expectedAttribsNamesValues);
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
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public T GetWhen(
            CancellationToken cancellationToken,
            string attributeName,
            string expectedAttributeValue)
        {
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            T elmt = GetWhen(cancellationToken, expected);
            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public T GetWhen(
           CancellationToken cancellationToken,
           params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues)
        {
            var expectedDic = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            T elmt = GetWhen(cancellationToken, expectedDic);
            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam
        /// and when the condition are met.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="driver">This AppiumDriver<IWebElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WebElement.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public T GetWhen(
           CancellationToken cancellationToken,
           Dictionary<string, string> expectedAttribsNamesValues)
        {
            T elmt = FindFirstElement();
            if (elmt == null) return default;
            if (!elmt.WaitUntil(cancellationToken, expectedAttribsNamesValues))
                return default;

            return elmt;
        }

        #endregion Get Methods

        #region Private

        private CancellationToken[] LinkCancellationTokens(params CancellationToken[] cancellationTokens)
        {
            var linkedTokens = new List<CancellationToken>();
            if (cancellationTokens.Length == 0 && DefaultCancellationToken == null)
            {
                throw new UninitializedDefaultCancellationTokenException();
            }

            if (DefaultCancellationToken != CancellationToken.None)
            {
                linkedTokens.Add(DefaultCancellationToken);
            }

            linkedTokens.AddRange(cancellationTokens);
            return linkedTokens.ToArray();
        }

        private T FindFirstElement()
        {
            return AppiumDriver
                .FindElements(Locator, Value)
                .FirstOrDefault();
        }

        #endregion Private
    }
}