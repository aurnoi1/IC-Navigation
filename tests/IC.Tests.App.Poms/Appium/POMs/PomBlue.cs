using IC.Navigation;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.Extensions.Appium.WindowsDriver.Enums;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using System;
using System.Collections.Generic;

namespace IC.Tests.App.Poms.Appium.POMs
{
    [Aliases("blue page")]
    public class PomBlue : PomBase
    {
        private readonly IFacade session;

        public PomBlue(in IFacade session) : base(session)
        {
            this.session = session;
            RegisterObserver(session);
        }

        #region Controls

        /// <summary>
        /// WDSearchParam to find the title of this page.
        /// </summary>
        [Aliases("title")]
        public WDSearchParam UILblTitleParam => new WDSearchParam(WDLocators.AutomationId, "TitleBlue");

        /// <summary>
        /// WDSearchParam to find a control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public WDSearchParam UIBtnBackParam => new WDSearchParam(WDLocators.AutomationId, "BtnBack");

        /// <summary>
        /// WDSearchParam to find a control to open the yellow page.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public WDSearchParam BtnOpenYellowViewParam => new WDSearchParam(WDLocators.AutomationId, "BtnOpenYellowView");

        #endregion Controls

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        public override INavigableStatus PublishStatus()
        {
            bool isDisplayed = session.WindowsDriver.Get(UILblTitleParam) != null;
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
                { session.PomMenu, () => session.WindowsDriver.Get(UIBtnBackParam).Click() },
                { session.PomYellow, () => session.WindowsDriver.Get(BtnOpenYellowViewParam).Click() },
            };
        }
    }
}