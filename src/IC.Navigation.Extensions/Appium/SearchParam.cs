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

        #region Private

        private CancellationToken[] LinkCancellationTokens(params CancellationToken[] cancellationTokens)
        {
            throw new NotImplementedException("To Test");
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