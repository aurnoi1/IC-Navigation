using IC.Navigation.Extensions.Appium.WindowsDriver.Enums;
using IC.Navigation.Extensions.Appium.WindowsDriver.Interfaces;

namespace IC.Navigation.Extensions.Appium.WindowsDriver
{
    public class WDSearchParam : IWDSearchParam
    {
        public WDSearchParam()
        {

        }

        public WDSearchParam(string locator, string value)
        {

        }

        public WDSearchParam(WDLocators wDLocators, string value)
        {

        }

        public string Locator { get; set; }
        public string Value { get; set; }
    }
}