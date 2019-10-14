using IC.Navigation.Extensions.Appium.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

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
        public AppiumDriver<T> AppiumDriver { get; set; }
    }
}