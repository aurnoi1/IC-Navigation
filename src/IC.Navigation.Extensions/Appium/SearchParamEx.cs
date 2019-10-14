using IC.Navigation.Extensions.Appium.Interfaces;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;

namespace IC.Navigation.Extensions.Appium
{
    public static class SearchParamEx
    {
        public static T Find<T>(
            this SearchParam<T> searchParam,
            TimeSpan timeout) where T : IWebElement
        {
            using (var cts = new CancellationTokenSource(timeout))
            {
                var elmt = Get(searchParam, cts.Token);
                if (cts.Token.IsCancellationRequested)
                    throw new TimeoutException("The timeout has been reached.");

                return elmt;
            }
        }

        public static T Get<T>(this SearchParam<T> searchParam) where T : IWebElement
        {
            return FindFirstElement(searchParam);
        }

        public static T Get<T>(
            this SearchParam<T> searchParam,
            CancellationToken cancellationToken) where T : IWebElement
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