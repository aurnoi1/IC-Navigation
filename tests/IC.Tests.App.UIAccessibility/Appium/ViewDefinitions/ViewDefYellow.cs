using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Tests.App.UIAccessibility.Appium.ViewDefinitions
{
    public abstract class ViewDefYellow : IViewDefYellow
    {
        protected ViewDefYellow(in IUIAccess session) => this.session = session;

        private IUIAccess session;

        /// <summary>
        /// The tile of this view.
        /// </summary>
        [UIArtefact("title")] // explicitly same than other views for test.
        public WindowsElement UITitle => session.WindowsDriver.FindElementByAccessibilityId(
            "TitleYellow", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the previous page.
        /// </summary>
        [UIArtefact("button to go back to the previous view")]
        public WindowsElement UIBtnBack => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnBack", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the previous page.
        /// </summary>
        [UIArtefact("button to open menu view")]
        public WindowsElement UIBtnOpenMenuView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenMenuView", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));
    }
}