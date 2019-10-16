using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace IC.Navigation.Extensions.Appium.Interfaces
{
    public interface ISearchParam<T> where T : IWebElement
    {
        /// <summary>
        /// Locator to find the WindowsElement.
        /// </summary>
        string Locator { get; set; }

        /// <summary>
        /// Value of the parameter.
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// The AppiumDriver used to find the WindowsElement.
        /// </summary>
        public AppiumDriver<T> AppiumDriver { get; }

        /// <summary>
        /// The default CancelleationToken to interrupt a search of the WindowsElement.
        /// </summary>
        CancellationToken DefaultCancellationToken { get; }
    }
}