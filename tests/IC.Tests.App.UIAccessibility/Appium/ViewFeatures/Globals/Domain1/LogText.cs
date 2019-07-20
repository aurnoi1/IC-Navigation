using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Navigation.CoreExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IC.Tests.App.UIAccessibility.Appium.ViewFeatures.Globals.Domain1
{
    public static class LogText
    {
        /// <summary>
        /// Log a text.
        /// This method is domain specific and should be accessible from any INavigable
        /// (as far they have path to next INavigable).
        /// </summary>
        /// <param name="origin">this INavigable.</param>
        /// <param name="text">The text to log.</param>
        /// <returns>The IVewMenu where the log was enter.</returns>
        public static IViewMenu Log(this INavigable origin, string text)
        {
            IFacade mySession = origin.Session as IFacade;
            IViewMenu viewMenu = mySession.ViewMenu;
            origin.GoTo(viewMenu)
                .Do(() =>
                {
                    viewMenu.EnterText(text);
                });

            return viewMenu;
        }
    }
}
