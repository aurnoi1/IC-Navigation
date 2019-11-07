using IC.Navigation.Extensions.Appium.Interfaces;
using IC.Navigation.Extensions.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IC.Navigation.Extensions.Appium
{
    public class SearchProperties<W> : ISearchProperties<W> where W : IWebElement
    {
        private const string timeoutExceptionMessage = "The timeout has been reached before the Element could be found.";

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
            IFindsByFluentSelector<W> appiumDriver,
            CancellationToken defaultCancellationToken = default)
        {
            Locator = locator;
            Value = value;
            WebDriver = appiumDriver;
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
        public IFindsByFluentSelector<W> WebDriver { get; private set; }

        /// <summary>
        /// The default CancellationToken to interrupt a search of the WindowsElement.
        /// </summary>
        public CancellationToken DefaultCancellationToken { get; private set; }

        #region Find Methods

        /// <summary>
        /// Search the first WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens used to stop waiting for the control to be found.
        /// They will be linked to the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement.</returns>
        /// <exception cref="OperationCanceledException">Thrown when any CancellationToken is cancelled.</exception>
        public W Find(params CancellationToken[] cancellationTokens)
        {
            var linkedTokens = LinkCancellationTokens(cancellationTokens);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            var elmt = Get(linkedTokenSource.Token);
            linkedTokenSource.Token.ThrowIfCancellationRequested();
            return elmt;
        }

        /// <summary>
        /// Search the first WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found
        /// and to its attributes to match expected values.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement.</returns>
        /// <exception cref="TimeoutException">Thrown when any timeout is reached before WebElement is found.</exception>
        /// <exception cref="OperationCanceledException">Thrown when any timeout is reached before
        /// the expected attributes match the expected values.
        /// </exception>
        public W Find(
            TimeSpan timeout,
            params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues)
        {
            using var cts = new CancellationTokenSource(timeout);
            var linkedTokens = LinkCancellationTokens(cts.Token);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            var webElement = Get(linkedTokenSource.Token);
            if (linkedTokenSource.Token.IsCancellationRequested)
            {
                throw new TimeoutException(timeoutExceptionMessage);
            }

            webElement.ContinueWhen(linkedTokenSource.Token, expectedAttribsNamesValues);
            return webElement;
        }

        /// <summary>
        /// Search the first WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement.</returns>
        /// <exception cref="TimeoutException">Thrown when any timeout is reached before WebElement is found.</exception>
        public W Find(TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);
            var linkedTokens = LinkCancellationTokens(cts.Token);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            var elmt = Get(linkedTokenSource.Token);
            if (linkedTokenSource.Token.IsCancellationRequested)
            {
                throw new TimeoutException(timeoutExceptionMessage);
            }

            return elmt;
        }

        #endregion Find Methods

        #region Get Methods

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// <see cref="DefaultCancellationToken"/> will be used if defined.
        /// </summary>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public W Get()
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
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public W Get(TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);
            var linkedTokens = LinkCancellationTokens(cts.Token);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            return Get(linkedTokenSource.Token);
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens used to stop waiting for the control to be found.
        /// They will be linked to the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public W Get(params CancellationToken[] cancellationTokens)
        {
            var linkedTokens = LinkCancellationTokens(cancellationTokens);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            return Get(linkedTokenSource.Token);
        }

        #endregion Get Methods

        #region GetWhen Methods

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties
        /// and when the attribute name and value match expected.
        /// The <see cref="DefaultCancellationToken"/> cannot be <c>null</c> or <c>None</c>.
        /// </summary>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        /// <exception cref="UninitializedDefaultCancellationTokenException">Thrown when DefaultCancellationToken is uninitialized.</exception>
        public W GetWhen(
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
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties
        /// and when the attribute name and value match expected.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public W GetWhen(
            TimeSpan timeout,
            string attributeName,
            string expectedAttributeValue)
        {
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            return GetWhen(timeout, expected);
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties
        /// and when the attribute names and values match expected.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public W GetWhen(
           TimeSpan timeout,
           params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues)
        {
            var expected = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            return GetWhen(timeout, expected);
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties
        /// and when the attribute names and values match expected.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the condition to meet.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public W GetWhen(
            TimeSpan timeout,
            Dictionary<string, string> expectedAttribsNamesValues)
        {
            using CancellationTokenSource cts = new CancellationTokenSource(timeout);
            return GetWhen(cts.Token, expectedAttribsNamesValues);
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties
        /// and when the attribute names and values match expected.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.
        /// This CancellationToken will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <param name="attributeName">The attribute name (case sensitive).</param>
        /// <param name="expectedAttributeValue">The expected attribute value (case sensitive).</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public W GetWhen(
            CancellationToken cancellationToken,
            string attributeName,
            string expectedAttributeValue)
        {
            var expected = new Dictionary<string, string>();
            expected.Add(attributeName, expectedAttributeValue);
            W elmt = GetWhen(cancellationToken, expected);
            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties
        /// and when the attribute names and values match expected.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.
        /// This CancellationToken will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names and expected values as Value Tuples.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public W GetWhen(
           CancellationToken cancellationToken,
           params (string attributeName, string expectedAttributeValue)[] expectedAttribsNamesValues)
        {
            var expectedDic = expectedAttribsNamesValues.ToDictionary(x => x.attributeName, x => x.expectedAttributeValue);
            W elmt = GetWhen(cancellationToken, expectedDic);
            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="W"/> matching the SearchProperties
        /// and when the attribute names and values match expected.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken used to stop to wait for the condition to meet.
        /// This CancellationToken will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <param name="expectedAttribsNamesValues">The attributes names as keys and the expected values.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c></returns>
        public W GetWhen(
           CancellationToken cancellationToken,
           Dictionary<string, string> expectedAttribsNamesValues)
        {
            var linkedTokens = LinkCancellationTokens(cancellationToken);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            W elmt = Get(linkedTokenSource.Token);
            if (elmt == null) return default;
            if (!elmt.WaitUntil(linkedTokenSource.Token, expectedAttribsNamesValues))
                return default;

            return elmt;
        }

        #endregion GetWhen Methods

        #region Private

        private W Get(CancellationToken linkedCancellationTokens)
        {
            while (!linkedCancellationTokens.IsCancellationRequested)
            {
                var match = FindFirstElement();
                if (match != null) return match;
            }

            return default;
        }

        /// <summary>
        /// Link CancellationToken pass as parameter and the DefaultCancellationToken if initialized.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens to link.</param>
        /// <returns>The linked CancellationTokens.</returns>
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

        private W FindFirstElement()
        {
            return WebDriver
                .FindElements(Locator, Value)
                .FirstOrDefault();
        }

        #endregion Private
    }
}