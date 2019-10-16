using IC.Navigation.Extensions.Appium.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Linq;
using System.Threading;

namespace IC.Navigation.Extensions.Appium
{
    public class SearchParam<T> : ISearchParam<T> where T : IWebElement
    {
        public SearchParam()
        {
        }

        public SearchParam(string locator, string value, AppiumDriver<T> appiumDriver)
        {
            Locator = locator;
            Value = value;
            AppiumDriver = appiumDriver;
        }

        public string Locator { get; set; }
        public string Value { get; set; }
        public AppiumDriver<T> AppiumDriver { get; private set; }

        /// <summary>
        /// Search the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="searchParam">This SearchParam.</param>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.</param>
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
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="searchParam">This SearchParam.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public T Get()
        {
            return FindFirstElement();
        }

        /// <summary>
        /// Get the first WebElement of type <typeparamref name="T"/> matching the SearchParam.
        /// </summary>
        /// <typeparam name="T">The type of WebElement.</typeparam>
        /// <param name="searchParam">This SearchParam.</param>
        /// <param name="cancellationToken">The CancellationToken used to stop waiting for the control to be found.</param>
        /// <returns>The first matching WebElement, otherwise <c>null</c>.</returns>
        public T Get(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var match = FindFirstElement();
                if (match != null) return match;
            }

            return default;
        }

        #region Private

        private T FindFirstElement()
        {
            return AppiumDriver
                .FindElements(Locator, Value)
                .FirstOrDefault();
        }

        #endregion Private
    }
}