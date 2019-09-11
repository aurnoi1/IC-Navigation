using IC.Navigation.Extensions.Appium.WindowsDriver.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IC.Navigation.Extensions.Appium.WindowsDriver
{
    public static class IWindowsDriverSessionEx
    {
        /// <summary>
        /// Find a WindowsElement in the last known INavigable, based on its attribute Aliases, on the last known view.
        /// </summary>
        /// <param name="winDriverSession">This IWindowsDriverSession.</param>
        /// <param name="alias">The matching alias.</param>
        /// <returns>The WindowsElement if found, otherwise <c>null</c>.</returns>
        public static WindowsElement FindElementByAliasesInLastINavigable(this IWindowsDriverSession winDriverSession, string alias)
        {
            WindowsElement match = null;
            var prop = GetLastINavigableAliasesAndProperties(winDriverSession).SingleOrDefault(x => x.Key.Values.Contains(alias)).Value;
            if (prop != null)
            {
                var wdSearchParam = prop.GetValue(winDriverSession.Last) as WDSearchParam;
                match = winDriverSession.WindowsDriver.Get(wdSearchParam);
            }

            return match;
        }

        /// <summary>
        /// Get the properties and the associated Aliases attribute, of last known INavigable.
        /// </summary>
        /// <param name="winDriverSession">This IWindowsDriverSession.</param>
        /// <returns>The properties and the associated Aliases of last INavigable.</returns>
        public static Dictionary<Aliases, PropertyInfo> GetLastINavigableAliasesAndProperties(this IWindowsDriverSession winDriverSession)
        {
            Dictionary<Aliases, PropertyInfo> propertyInfos = new Dictionary<Aliases, PropertyInfo>();
            var properties = winDriverSession.Last.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    if (attr is Aliases uIArtefact && prop.PropertyType.Equals(typeof(WDSearchParam)))
                    {
                        propertyInfos.Add(uIArtefact, prop);
                    }
                }
            }

            return propertyInfos;
        }
    }
}