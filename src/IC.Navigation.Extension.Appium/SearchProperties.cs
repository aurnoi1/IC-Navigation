using IC.Navigation.Extension.Appium.Exceptions;
using IC.Navigation.Extension.Appium.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IC.Navigation.Extension.Appium
{
    public class SearchProperties<W> : ISearchProperties<W> where W : IWebElement
    {
        private const string timeoutExceptionMessage = "The timeout has been reached before the Element could be found.";

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchProperties"/> class.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="value"></param>
        /// <param name="webDriver"></param>
        /// <param name="defaultCancellationToken"></param>
        public SearchProperties(
            string locator,
            string value,
            IFindsByFluentSelector<W> webDriver,
            CancellationToken defaultCancellationToken)
            : this(locator, value, webDriver, 0, defaultCancellationToken)
        {
        }

        public SearchProperties(
            string locator,
            string value,
            IFindsByFluentSelector<W> webDriver,
            int index)
            : this(locator, value, webDriver, index, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchProperties"/> class.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="value"></param>
        /// <param name="webDriver"></param>
        /// <param name="defaultCancellationToken"></param>
        public SearchProperties(
            string locator,
            string value,
            IFindsByFluentSelector<W> webDriver) : this(locator, value, webDriver, defaultCancellationToken: default)
        {
        }

        public SearchProperties(
            string locator,
            string value,
            IFindsByFluentSelector<W> webDriver,
            int index,
            CancellationToken defaultCancellationToken)
        {
            Locator = locator ?? throw new ArgumentNullException(nameof(locator));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            WebDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
            Index = index;
            DefaultCancellationToken = defaultCancellationToken;
        }

        /// <summary>
        /// Locator to find the WebElement.
        /// </summary>
        public string Locator { get; set; }

        /// <summary>
        /// Value of the Locator.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The WebDriver used to find the WebElement.
        /// </summary>
        public IFindsByFluentSelector<W> WebDriver { get; private set; }

        /// <summary>
        /// The default CancellationToken to interrupt a search of the WebElement.
        /// </summary>
        public CancellationToken DefaultCancellationToken { get; private set; }

        /// <summary>
        /// The index of the WebElement in the collection of matching WebElements.
        /// </summary>
        public int Index { get; set; }

        #region Find Methods

        /// <summary>
        /// Search the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// The <see cref="DefaultCancellationToken"/> is mandatory when using this method.
        /// <returns>The matching WebElement.</returns>
        /// <exception cref="OperationCanceledException">Thrown when any CancellationToken is cancelled.</exception>
        /// <exception cref="UninitializedDefaultCancellationTokenException">Thrown when no CancellationToken is initialized.</exception>
        public W Find() => Find(cancellationTokens: null);

        /// <summary>
        /// Search the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens used to stop waiting for the control to be found.
        /// They will be linked to the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The matching WebElement.</returns>
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
        /// Search the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The matching WebElement.</returns>
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
        /// Get the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// <see cref="DefaultCancellationToken"/> will be used if defined.
        /// </summary>
        /// <returns>The matching WebElement, otherwise <c>null</c>.</returns>
        public W Get()
        {
            if (DefaultCancellationToken == null || DefaultCancellationToken == CancellationToken.None)
            {
                return FindElement();
            }
            else
            {
                return Get(DefaultCancellationToken);
            }
        }

        /// <summary>
        /// Get the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The matching WebElement, otherwise <c>null</c>.</returns>
        public W Get(TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);
            var linkedTokens = LinkCancellationTokens(cts.Token);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            return Get(linkedTokenSource.Token);
        }

        /// <summary>
        /// Get the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens used to stop waiting for the control to be found.
        /// They will be linked to the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The matching WebElement, otherwise <c>null</c>.</returns>
        public W Get(params CancellationToken[] cancellationTokens)
        {
            var linkedTokens = LinkCancellationTokens(cancellationTokens);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            return Get(linkedTokenSource.Token);
        }

        #endregion Get Methods

        #region Private

        private W Get(CancellationToken linkedCancellationTokens)
        {
            while (!linkedCancellationTokens.IsCancellationRequested)
            {
                var match = FindElement();
                if (match != null) return match;
            }

            return default;
        }

        /// <summary>
        /// Link CancellationToken pass as parameter and the DefaultCancellationToken if initialized.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens to link.</param>
        /// <returns>The linked CancellationTokens.</returns>
        /// <exception cref="UninitializedDefaultCancellationTokenException">Thrown when no CancellationToken is initialized.</exception>
        private CancellationToken[] LinkCancellationTokens(params CancellationToken[] cancellationTokens)
        {
            var linkedTokens = new List<CancellationToken>();
            if (cancellationTokens == null || cancellationTokens.Length == 0)
            {
                if (DefaultCancellationToken == null || DefaultCancellationToken == CancellationToken.None)
                {
                    throw new UninitializedDefaultCancellationTokenException();
                }
            }

            if (DefaultCancellationToken != CancellationToken.None)
            {
                linkedTokens.Add(DefaultCancellationToken);
            }

            if (cancellationTokens != null)
            {
                linkedTokens.AddRange(cancellationTokens);
            }

            return linkedTokens.ToArray();
        }

        private W FindElement()
        {
            return WebDriver
                .FindElements(Locator, Value)
                .ElementAtOrDefault(Index);
        }

        #endregion Private
    }
}