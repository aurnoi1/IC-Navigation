using IC.Navigation.Extensions.Appium.WindowsDriver.Enums;
using IC.Navigation.Extensions.Appium.WindowsDriver.Interfaces;
using System;

namespace IC.Navigation.Extensions.Appium.WindowsDriver
{
    public class WDSearchParam : IWDSearchParam
    {
        public WDSearchParam()
        {

        }

        public WDSearchParam(string locator, string value)
        {
            Locator = locator;
            value = Value;
        }

        public WDSearchParam(WDLocators wDLocators, string value)
        {
            Locator = Enum.GetName(typeof(WDLocators), wDLocators);
            Value = value;
        }

        public string Locator { get; set; }
        public string Value { get; set; }
    }
}