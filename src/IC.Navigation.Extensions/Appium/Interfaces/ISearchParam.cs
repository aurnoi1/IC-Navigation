using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

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

        public AppiumDriver<T> AppiumDriver { get; }
    }
}