using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
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
        public SearchParam UIBtnNotImplementedParam => new SearchParam(WDLocators.AutomationId, "NotImplemented");

        /// <summary>
        /// WDSearchParam to find the tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public SearchParam UITitleParam => new SearchParam(WDLocators.AutomationId, "TitleMenu");

        /// <summary>
        /// WDSearchParam to find a control to open the BlueView.
        /// </summary>
        [Aliases("button to open the blue page")]
        public SearchParam UIBtnOpenBlueViewParam => new SearchParam(WDLocators.AutomationId, "BtnOpenBlueView");

        /// <summary>
        /// WDSearchParam to find a control to open the RedView.
        /// </summary>
        [Aliases("button to open the red page")]
        public SearchParam UIBtnOpenRedViewParam => new SearchParam(WDLocators.AutomationId, "BtnOpenRedView");

        /// <summary>
        /// WDSearchParam to find a control to open the RedView.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public SearchParam UIBtnOpenYellowViewParam => new SearchParam(WDLocators.AutomationId, "BtnOpenYellowView");

        /// <summary>
        /// WDSearchParam to find a control where text can be enter.
        /// </summary>
        [Aliases("box where enter text")]
        public SearchParam UITxtBoxImportantMessageParam => new SearchParam(WDLocators.AutomationId, "TxtBoxImportantMessage");

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
                { session.PomBlue, (ct) => session.WindowsDriver.Find(UIBtnOpenBlueViewParam, ct).Click() },
                { session.PomRed, (ct) => session.WindowsDriver.Find(UIBtnOpenRedViewParam, ct).Click() },
                { session.PomYellow, (ct) => session.WindowsDriver.Find(UIBtnOpenYellowViewParam, ct).Click() },
            };
        }

        /// <summary>
        /// Enter a text in the UITxtBoxImportantMessage.
        /// </summary>
        /// <param name="text">The text to enter.</param>
        public void EnterText(string text)
        {
            session.WindowsDriver.Get(UITxtBoxImportantMessageParam).SendKeys(text);
        }
    }
}