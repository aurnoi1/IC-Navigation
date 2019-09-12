using IC.Navigation.Extensions.Appium.Interfaces;
using IC.Navigation.Extensions.Appium.WindowsDriver.Enums;
using IC.Navigation.Extensions.Appium.WindowsDriver.Interfaces;
using System;

namespace IC.Navigation.Extensions.Appium.WindowsDriver
{
    public class SearchParam : ISearchParam
    {
        public SearchParam()
        {

        }

        public SearchParam(string locator, string value)
        {
            Locator = locator;
            Value = value;
        }

        public string Locator { get; set; }
        public string Value { get; set; }
    }
}