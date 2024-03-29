﻿using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Tests.App.UIAccessibility.Appium.ViewDefinitions
{
    public abstract class ViewDefRed : IViewDefRed
    {
        protected ViewDefRed(in IUIAccess session) => this.session = session;

        private IUIAccess session;

        /// <summary>
        /// The tile of this view.
        /// </summary>
        [UIArtefact("title")] // explicitly same than other views for test.
        public WindowsElement UITitle => session.WindowsDriver.FindElementByAccessibilityId(
            "TitleRed", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the previous page.
        /// </summary>
        [UIArtefact("button to go back to the previous view")]
        public WindowsElement UIBtnBack => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnBack", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the yellow view.
        /// </summary>
        [UIArtefact("button to open the yellow view")]
        public WindowsElement UIBtnOpenYellowView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenYellowView", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));
    }
}