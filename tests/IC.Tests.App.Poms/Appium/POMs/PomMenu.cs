﻿using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Appium.Interfaces;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Tests.App.Poms.Appium.POMs
{
    [Aliases("menu page")]
    public class PomMenu : PomBase
    {
        public PomMenu(in IFacade session) : base(session)
        {
            RegisterObserver(session);
        }

        #region Controls

        /// <summary>
        /// WDSearchParam to find a control NOT IMPLEMENTED only use for negative test.
        /// </summary>
        [Aliases("not implemented")]
        public SearchParam<WindowsElement> UIBtnNotImplemented => new SearchParam<WindowsElement>(WDLocators.AutomationId, "NotImplemented", session.WindowsDriver);

        /// <summary>
        /// WDSearchParam to find the tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public SearchParam<WindowsElement> UITitleParam => new SearchParam<WindowsElement>(WDLocators.AutomationId, "TitleMenu", session.WindowsDriver);

        /// <summary>
        /// WDSearchParam to find a control to open the BlueView.
        /// </summary>
        [Aliases("button to open the blue page")]
        public SearchParam<WindowsElement> UIBtnOpenBlueView => new SearchParam<WindowsElement>(WDLocators.AutomationId, "BtnOpenBlueView", session.WindowsDriver);

        /// <summary>
        /// WDSearchParam to find a control to open the RedView.
        /// </summary>
        [Aliases("button to open the red page")]
        public SearchParam<WindowsElement> UIBtnOpenRedViewP => new SearchParam<WindowsElement>(WDLocators.AutomationId, "BtnOpenRedView", session.WindowsDriver);

        /// <summary>
        /// WDSearchParam to find a control to open the RedView.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public SearchParam<WindowsElement> UIBtnOpenYellowView => new SearchParam<WindowsElement>(WDLocators.AutomationId, "BtnOpenYellowView", session.WindowsDriver);

        /// <summary>
        /// WDSearchParam to find a control where text can be enter.
        /// </summary>
        [Aliases("box where enter text")]
        public SearchParam<WindowsElement> UITxtBoxImportantMessage => new SearchParam<WindowsElement>(WDLocators.AutomationId, "TxtBoxImportantMessage", session.WindowsDriver);

        #endregion Controls

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        public override INavigableStatus PublishStatus()
        {
            bool isDisplayed = session.WindowsDriver.Get(UITitleParam) != null;
            NavigableStatus status = new NavigableStatus();
            status.Exists = isDisplayed;
            NotifyObservers(status);
            return status;
        }

        /// <summary>
        /// Gets a Dictionary of action to go to the next INavigable.
        /// </summary>
        /// <returns>A Dictionary of action to go to the next INavigable.</returns>
        public override Dictionary<INavigable, Action<CancellationToken>> GetActionToNext()
        {
            return new Dictionary<INavigable, Action<CancellationToken>>()
            {
                { session.PomBlue, (ct) => session.WindowsDriver.Find(UIBtnOpenBlueView, ct).Click() },
                { session.PomRed, (ct) => session.WindowsDriver.Find(UIBtnOpenRedViewP, ct).Click() },
                { session.PomYellow, (ct) => session.WindowsDriver.Find(UIBtnOpenYellowView, ct).Click() },
            };
        }

        /// <summary>
        /// Enter a text in the UITxtBoxImportantMessage.
        /// </summary>
        /// <param name="text">The text to enter.</param>
        public void EnterText(string text)
        {
            session.WindowsDriver.Get(UITxtBoxImportantMessage).SendKeys(text);
        }
    }
}