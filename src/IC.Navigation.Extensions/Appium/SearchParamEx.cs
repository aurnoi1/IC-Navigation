using IC.Navigation.Extensions.Appium.Interfaces;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;

namespace IC.Navigation.Extensions.Appium
{
    public static class SearchParamEx
    {
        /// <summary>
        /// Search the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="searchParam">This SearchParam.</param>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.</param>
        /// <returns>The first matching WebElement.</returns>
        /// <exception cref="TimeoutException">Thrown when timeout is reached before WebElement is found.</exception>
        public static T Find<T>(this SearchParam<T> searchParam, TimeSpan timeout) where T : IWebElement
        {
            using var cts = new CancellationTokenSource(timeout);
            var elmt = Get(searchParam, cts.Token);
            if (cts.Token.IsCancellationRequested)
            {
                throw new TimeoutException("The timeout has been reached.");
            }

            return elmt;
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="searchParam">This SearchParam.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public static T Get<T>(this SearchParam<T> searchParam) where T : IWebElement
        {
            return FindFirstElement(searchParam);
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="searchParam">This SearchParam.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop waiting for the control to be found.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public static T Get<T>(this SearchParam<T> searchParam, CancellationToken cancellationToken) where T : IWebElement
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var match = FindFirstElement(searchParam);
                if (match != null) return match;
            }

            return default;
        }

        #region Private

        private static T FindFirstElement<T>(ISearchParam<T> searchParam) where T : IWebElement
        {
            return searchParam.AppiumDriver.FindElements(searchParam.Locator, searchParam.Value).FirstOrDefault();
        }

        #endregion Private
    }
}