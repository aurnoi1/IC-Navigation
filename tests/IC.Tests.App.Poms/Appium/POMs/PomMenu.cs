using IC.Navigation;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.Extensions.Appium.WindowsDriver.Enums;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
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
        public WDSearchParam UIBtnNotImplemented => new WDSearchParam(WDLocators.AutomationId, "NotImplemented");

        /// <summary>
        /// The tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public WDSearchParam UITitle => new WDSearchParam(WDLocators.AutomationId, "TitleMenu");

        /// <summary>
        /// A control to open the BlueView.
        /// </summary>
        [Aliases("button to open the blue page")]
        public WDSearchParam UIBtnOpenBlueView => new WDSearchParam(WDLocators.AutomationId, "BtnOpenBlueView");

        /// <summary>
        /// A control to open the RedView.
        /// </summary>
        [Aliases("button to open the red page")]
        public WDSearchParam UIBtnOpenRedView => new WDSearchParam(WDLocators.AutomationId, "BtnOpenRedView");

        /// <summary>
        /// A control to open the RedView.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public WDSearchParam UIBtnOpenYellowView => new WDSearchParam(WDLocators.AutomationId, "BtnOpenYellowView");

        /// <summary>
        /// A control where text can be enter.
        /// </summary>
        [Aliases("box where enter text")]
        public WDSearchParam UITxtBoxImportantMessage => new WDSearchParam(WDLocators.AutomationId, "TxtBoxImportantMessage");

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
                { session.PomBlue, () => session.WindowsDriver.Get(UIBtnOpenBlueView).Click() },
                { session.PomRed, () => session.WindowsDriver.Get(UIBtnOpenRedView).Click() },
                { session.PomYellow, () => session.WindowsDriver.Get(UIBtnOpenYellowView).Click() },
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