﻿using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;

namespace IC.Tests.App.Poms.Appium.POMs
{
    [Aliases("red page")]
    public class PomRed : PomBase
    {
        private readonly IFacade session;

        public PomRed(in IFacade session) : base(session)
        {
            this.session = session;
            RegisterObserver(session);
        }

        #region Controls

        /// <summary>
        /// The tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public WindowsElement UITitle => session.WindowsDriver.FindElementByAccessibilityId(
            "TitleRed",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public WindowsElement UIBtnBack => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnBack",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the yellow page.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public WindowsElement UIBtnOpenYellowView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenYellowView",
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
            NotifyObservers(status);
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
                { session.PomMenu, () => UIBtnBack.Click() },
                { session.PomYellow, () => UIBtnOpenYellowView.Click() },
            };
        }
    }
}