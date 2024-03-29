﻿using OpenQA.Selenium.Appium.Windows;
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
        /// <param name="winDriverSession">This IWindowsDriverSession.</param>
        /// <param name="usageName">The matching usage name.</param>
        /// <returns>The WindowsElement if found, otherwise <c>null</c>.</returns>
        public static WindowsElement FindElementByUsageNameInLastINavigable(this IWindowsDriverSession winDriverSession, string usageName)
        {
            WindowsElement match = null;
            var prop = GetLastINavigableUIArtefactAndProperties(winDriverSession).SingleOrDefault(x => x.Key.UsageName == usageName).Value;
            if (prop != null)
            {
                match = prop.GetMethod.Invoke(winDriverSession.Last, null) as WindowsElement;
            }

            return match;
        }

        /// <summary>
        /// Get the properties and the associated UIArtefact attribute, of last known INavigable.
        /// </summary>
        /// <param name="winDriverSession">This IWindowsDriverSession.</param>
        /// <returns>The properties and the associated UIArtefact attribute, of last INavigable.</returns>
        public static Dictionary<UIArtefact, PropertyInfo> GetLastINavigableUIArtefactAndProperties(this IWindowsDriverSession winDriverSession)
        {
            Dictionary<UIArtefact, PropertyInfo> propertyInfos = new Dictionary<UIArtefact, PropertyInfo>();
            var properties = winDriverSession.Last.GetType().GetProperties();
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