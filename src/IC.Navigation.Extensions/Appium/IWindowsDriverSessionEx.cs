using IC.Navigation.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IC.Navigation.Extensions.Appium
{
    public static class IWindowsDriverSessionEx
    {
        /// <summary>
        /// Find a WindowsElement in the last known INavigable, based on its attribute UIArtefact.UsageName, on the last known view.
        /// </summary>
        /// <param name="winDriverBrowser">This IWindowsDriverBrowser.</param>
        /// <param name="usageName">The matching usage name.</param>
        /// <returns>The WindowsElement if found, otherwise <c>null</c>.</returns>
        public static WindowsElement FindElementByUsageNameInLastINavigable(this IWindowsDriverSession winDriverBrowser, string usageName)
        {
            WindowsElement match = null;
            var prop = GetLastINavigableUIArtefactAndProperties(winDriverBrowser).SingleOrDefault(x => x.Key.UsageName == usageName).Value;
            if (prop != null)
            {
                match = prop.GetMethod.Invoke(winDriverBrowser.Last, null) as WindowsElement;
            }

            return match;
        }

        /// <summary>
        /// Get the properties and the associated UIArtefact attribute, of last known INavigable.
        /// </summary>
        /// <param name="winDriverBrowser">This IWindowsDriverBrowser.</param>
        /// <returns>The properties and the associated UIArtefact attribute, of last INavigable.</returns>
        public static Dictionary<UIArtefact, PropertyInfo> GetLastINavigableUIArtefactAndProperties(this IWindowsDriverSession winDriverBrowser)
        {
            Dictionary<UIArtefact, PropertyInfo> propertyInfos = new Dictionary<UIArtefact, PropertyInfo>();
            var properties = winDriverBrowser.Last.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    UIArtefact uIArtefact = attr as UIArtefact;
                    if (uIArtefact != null && prop.PropertyType.Equals(typeof(WindowsElement)))
                    {
                        propertyInfos.Add(uIArtefact, prop);
                    }
                }
            }

            return propertyInfos;
        }
    }
}