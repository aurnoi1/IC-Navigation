using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace IC.Tests.App.UIAccessibility.Appium.ViewDefinitions
{
    public abstract class ViewDefMenu : IViewDefMenu
    {
        protected ViewDefMenu(in IFacade session) => this.session = session;

        private readonly IFacade session;
        
        /// <summary>
        /// A control NOT IMPLEMENTED only use for negative test.
        /// </summary>
        [UIArtifact("not implemented")]
        public WindowsElement UIBtnNotImplemented => session.WindowsDriver.FindElementByAccessibilityId(
            "NotImplemented", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// The tile of this view.
        /// </summary>
        [UIArtifact("title")] // explicitly same than other views for test.
        public WindowsElement UITitle => session.WindowsDriver.FindElementByAccessibilityId(
            "TitleMenu", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the BlueView.
        /// </summary>
        [UIArtifact("button to open the blue view")]
        public WindowsElement UIBtnOpenBlueView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenBlueView", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the RedView.
        /// </summary>
        [UIArtifact("button to open the red view")]
        public WindowsElement UIBtnOpenRedView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenRedView", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the RedView.
        /// </summary>
        [UIArtifact("button to open the yellow view")]
        public WindowsElement UIBtnOpenYellowView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenYellowView", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control where text can be enter.
        /// </summary>
        [UIArtifact("box where enter text")]
        public WindowsElement UITxtBoxImportantMessage => session.WindowsDriver.FindElementByAccessibilityId(
            "TxtBoxImportantMessage", 
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));
    }
}