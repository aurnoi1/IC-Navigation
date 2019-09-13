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
        public SearchParam UILblTitleParam => new SearchParam(WDLocators.AutomationId, "TitleBlue");

        /// <summary>
        /// WDSearchParam to find a control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public SearchParam UIBtnBackParam => new SearchParam(WDLocators.AutomationId, "BtnBack");

        /// <summary>
        /// WDSearchParam to find a control to open the yellow page.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public SearchParam BtnOpenYellowViewParam => new SearchParam(WDLocators.AutomationId, "BtnOpenYellowView");

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
        public override Dictionary<INavigable, Action<CancellationToken>> GetActionToNext()
        {
            return new Dictionary<INavigable, Action<CancellationToken>>()
            {
                { session.PomMenu, (ct) => session.WindowsDriver.Search(UIBtnBackParam, ct).Click() },
                { session.PomYellow, (ct) => session.WindowsDriver.Search(BtnOpenYellowViewParam, ct).Click() },
            };
        }
    }
}