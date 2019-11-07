using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using System.Threading;

namespace IC.Navigation.Extensions.Appium.Interfaces
{
    public interface ISearchProperties<W> where W : IWebElement
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
        /// The a WebDriver that implement IFindsByFluentSelector<W>.
        /// </summary>
        public IFindsByFluentSelector<W> WebDriver { get; }

        /// <summary>
        /// The default CancellationToken to interrupt a search of the WindowsElement.
        /// </summary>
        CancellationToken DefaultCancellationToken { get; }
    }
}