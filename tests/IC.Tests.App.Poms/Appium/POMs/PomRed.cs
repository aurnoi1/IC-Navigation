using IC.Navigation;
using IC.Navigation.Extensions;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Enums;
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
        public SearchParam UITitle => new SearchParam(WDLocators.AutomationId, "TitleRed");

        /// <summary>
        /// A control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public SearchParam UIBtnBack => new SearchParam(WDLocators.AutomationId, "BtnBack");

        /// <summary>
        /// A control to open the yellow page.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public SearchParam UIBtnOpenYellowView => new SearchParam(WDLocators.AutomationId, "BtnOpenYellowView");

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
                { session.PomMenu, () => wd.Get(UIBtnBack).Click() },
                { session.PomYellow, () => wd.Get(UIBtnOpenYellowView).Click() },
            };
        }
    }
}