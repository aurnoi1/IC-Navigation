using IC.Navigation.Extensions.Appium.Interfaces;
using OpenQA.Selenium;
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
        public static WindowsElement FindElementByAliasesInLastINavigable(this IRemoteDriverSession<WindowsDriver<WindowsElement>> winDriverSession, string alias)
        {
            WindowsElement match = null;
            var prop = GetLastINavigableAliasesAndProperties(winDriverSession).SingleOrDefault(x => x.Key.Values.Contains(alias)).Value;
            if (prop != null)
            {
                var wdSearchProperties = prop.GetValue(winDriverSession.Last) as SearchProperties<IWebElement>;
                match = wdSearchProperties.Get() as WindowsElement;
            }

            return match;
        }

        /// <summary>
        /// Get the properties and the associated Aliases attribute, of last known INavigable.
        /// </summary>
        /// <param name="winDriverSession">This IWindowsDriverSession.</param>
        /// <returns>The properties and the associated Aliases of last INavigable.</returns>
        public static Dictionary<Aliases, PropertyInfo> GetLastINavigableAliasesAndProperties(this IRemoteDriverSession<WindowsDriver<WindowsElement>> winDriverSession)
        {
            Dictionary<Aliases, PropertyInfo> propertyInfos = new Dictionary<Aliases, PropertyInfo>();
            var properties = winDriverSession.Last.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    var g = prop.PropertyType.GenericTypeArguments.Single();
                    if (attr is Aliases uIArtefact && g.IsAssignableFrom(typeof(WindowsElement)))
                    {
                        propertyInfos.Add(uIArtefact, prop);
                    }
                }
            }

            return propertyInfos;
        }
    }
}