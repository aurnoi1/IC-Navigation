using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;

namespace IC.Tests.App.Poms.Appium.POMs
{
    [Aliases("menu page")]
    public class PomMenu : PomBase
    {
        private readonly IFacade session;

        public PomMenu(in IFacade session) : base(session)
        {
            this.session = session;
            RegisterObserver(session);
        }

        #region Controls

        /// <summary>
        /// A control NOT IMPLEMENTED only use for negative test.
        /// </summary>
        [Aliases("not implemented")]
        public WindowsElement UIBtnNotImplemented => session.WindowsDriver.FindElementByAccessibilityId(
            "NotImplemented",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// The tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public WindowsElement UITitle => session.WindowsDriver.FindElementByAccessibilityId(
            "TitleMenu",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the BlueView.
        /// </summary>
        [Aliases("button to open the blue page")]
        public WindowsElement UIBtnOpenBlueView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenBlueView",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the RedView.
        /// </summary>
        [Aliases("button to open the red page")]
        public WindowsElement UIBtnOpenRedView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenRedView",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the RedView.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public WindowsElement UIBtnOpenYellowView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenYellowView",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control where text can be enter.
        /// </summary>
        [Aliases("box where enter text")]
        public WindowsElement UITxtBoxImportantMessage => session.WindowsDriver.FindElementByAccessibilityId(
            "TxtBoxImportantMessage",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        #endregion Controls

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        public override INavigableStatus PublishStatus()
        {
            bool isDisplayed = UITitle != null;
            NavigableStatus status = new NavigableStatus();
            status.Exists = isDisplayed;
            INavigableEventArgs args = new NavigableEventArgs() { NavigableStatus = status };
            NotifyObservers(args);
            return status;
        }

        /// <summary>
        /// Gets a Dictionary of action to go to the next INavigable.
        /// </summary>
        /// <returns>A Dictionary of action to go to the next INavigable.</returns>
        public override Dictionary<INavigable, Action> GetActionToNext()
        {
            return new Dictionary<INavigable, Action>()
            {
                { session.PomBlue, () => UIBtnOpenBlueView.Click() },
                { session.PomRed, () => UIBtnOpenRedView.Click() },
                { session.PomYellow, () => UIBtnOpenYellowView.Click() },
            };
        }

        /// <summary>
        /// Enter a text in the UITxtBoxImportantMessage.
        /// </summary>
        /// <param name="text">The text to enter.</param>
        public void EnterText(string text)
        {
            UITxtBoxImportantMessage.SendKeys(text);
        }
    }
}