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
    public class SearchProperties<T> : ISearchProperties<T> where T : IWebElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchProperties"/> class.
        /// </summary>
        public SearchProperties()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchProperties"/> class.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="value"></param>
        /// <param name="appiumDriver"></param>
        /// <param name="defaultCancellationToken"></param>
        public SearchProperties(
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

        /// <summary>
        /// Locator to find the WindowsElement.
        /// </summary>
        public string Locator { get; set; }

        /// <summary>
        /// Value of the parameter.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The AppiumDriver used to find the WindowsElement.
        /// </summary>
        public AppiumDriver<T> AppiumDriver { get; private set; }

        /// <summary>
        /// The default CancellationToken to interrupt a search of the WindowsElement.
        /// </summary>
        public CancellationToken DefaultCancellationToken { get; private set; }

        #region Find Methods

        /// <summary>
        /// Search the first WebElement of type <typeparamref name="T"/> matching the SearchProperties.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens used to stop waiting for the control to be found.
        /// They will be linked to the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement.</returns>
        /// <exception cref="OperationCanceledException">Thrown when any CancellationToken is cancelled.</exception>
        public T Find(params CancellationToken[] cancellationTokens)
        {
            var linkedTokens = LinkCancellationTokens(cancellationTokens);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            var elmt = Get(linkedTokenSource.Token);
            linkedTokenSource.Token.ThrowIfCancellationRequested();
            return elmt;
        }

        /// <summary>
        /// Search the first WebElement of type <typeparamref name="T"/> matching the SearchProperties.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement.</returns>
        /// <exception cref="TimeoutException">Thrown when timeout is reached before WebElement is found.</exception>
        /// <exception cref="OperationCanceledException">Thrown when the <see cref="DefaultCancellationToken"/> is cancelled.</exception>
        public T Find(TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);
            var linkedTokens = LinkCancellationTokens(cts.Token);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            var elmt = Get(linkedTokenSource.Token);
            if (cts.Token.IsCancellationRequested)
            {
                throw new TimeoutException("The timeout has been reached.");
            }

            DefaultCancellationToken.ThrowIfCancellationRequested();
            return elmt;
        }

        #endregion Find Methods

        #region Get Methods

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties.
        /// <see cref="DefaultCancellationToken"/> will be used if defined.
        /// </summary>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public T Get()
        {
            if (DefaultCancellationToken == null || DefaultCancellationToken == CancellationToken.None)
            {
                return FindFirstElement();
            }
            else
            {
                return Get(DefaultCancellationToken);
            }
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public T Get(TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);
            var linkedTokens = LinkCancellationTokens(cts.Token);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            return Get(linkedTokenSource.Token);
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens used to stop waiting for the control to be found.
        /// They will be linked to the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public T Get(params CancellationToken[] cancellationTokens)
        {
            var linkedTokens = LinkCancellationTokens(cancellationTokens);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            return Get(linkedTokenSource.Token);
        }

        #endregion Get Methods

        #region GetWhen Methods

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties 
        /// and when the condition is met.
        /// The <see cref="DefaultCancellationToken"/> cannot be <c>null</c> or <c>None</c>.
        /// </summary>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        /// <exception cref="UninitializedDefaultCancellationTokenException">Thrown when DefaultCancellationToken is uninitialized.</exception>
        public T GetWhen(
            string attributeName,
            string expectedAttributeValue)
        {
            if (DefaultCancellationToken == null || DefaultCancellationToken == CancellationToken.None)
            {
                throw new UninitializedDefaultCancellationTokenException();
            }

            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            return GetWhen(DefaultCancellationToken, expected);
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties
        /// and when the condition is met.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
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
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties
        /// and when the condition are met.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
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
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties
        /// and when the condition are met.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public T GetWhen(
            TimeSpan timeout,
            Dictionary<string, string> expectedAttribsNamesValues)
        {
            using CancellationTokenSource cts = new CancellationTokenSource(timeout);
            return GetWhen(cts.Token, expectedAttribsNamesValues);
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties
        /// and when the condition are met.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.
        /// This CancellationToken will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
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
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties
        /// and when the condition are met.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.
        /// This CancellationToken will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
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
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchProperties
        /// and when the condition are met.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.
        /// This CancellationToken will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public T GetWhen(
           CancellationToken cancellationToken,
           Dictionary<string, string> expectedAttribsNamesValues)
        {
            var linkedTokens = LinkCancellationTokens(cancellationToken);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            T elmt = FindFirstElement();
            if (elmt == null) return default;
            if (!elmt.WaitUntil(linkedTokenSource.Token, expectedAttribsNamesValues))
                return default;

            return elmt;
        }

        #endregion GetWhen Methods

        #region Private

        private T Get(CancellationToken linkedCancellationTokens)
        {
            while (!linkedCancellationTokens.IsCancellationRequested)
            {
                var match = FindFirstElement();
                if (match != null) return match;
            }

            return default;
        }

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