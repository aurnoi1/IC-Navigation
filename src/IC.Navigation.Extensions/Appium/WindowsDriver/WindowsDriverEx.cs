using IC.Navigation.Extensions.Appium.WindowsDriver.Enums;
using IC.Navigation.Extensions.Appium.WindowsDriver.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Linq;

namespace IC.Navigation.Extensions.Appium.WindowsDriver
{
    public static class WindowsDriverEx
    {
        /// <summary>
        /// Get the WindowsElement.
        /// </summary>
        /// <param name="windowsDriver">This WindowsDriver<WindowsElement>.</param>
        /// <param name="searchParam">The SearchParam to use to find the WindowsElement.</param>
        /// <returns>The matching WindowsElement.</returns>
        /// <exception cref="OpenQA.Selenium.WebDriverException">Thrown when element is not found.</exception>
        public static WindowsElement Get(this WindowsDriver<WindowsElement> windowsDriver, IWDSearchParam searchParam)
        {
            return FindWindowsElement(windowsDriver, searchParam);
        }

        private static WindowsElement FindWindowsElement(WindowsDriver<WindowsElement> windowsDriver, IWDSearchParam searchParam)
        {
            switch (Enum.Parse(typeof(WDLocators), searchParam.Locator))
            {
                case WDLocators.AutomationId:
                    return windowsDriver.FindElementsByAccessibilityId(searchParam.Value).FirstOrDefault();

                case Enums.WDLocators.ClassName:
                    return windowsDriver.FindElementsByClassName(searchParam.Value).FirstOrDefault();

                case Enums.WDLocators.Name:
                    return windowsDriver.FindElementsByName(searchParam.Value).FirstOrDefault();

                default:
                    throw new Exception($"Unknown locator: {searchParam.Locator}.");
            }
        }
    }
}