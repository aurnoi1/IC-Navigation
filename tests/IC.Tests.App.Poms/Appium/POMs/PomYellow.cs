using IC.Navigation;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.Extensions.Appium.WindowsDriver.Enums;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using System;
using System.Collections.Generic;

namespace IC.Tests.App.Poms.Appium.POMs
{
    [Aliases("yellow page")]
    public class PomYellow : PomBase
    {
        private readonly IFacade session;

        public PomYellow(in IFacade session) : base(session)
        {
            this.session = session;
            RegisterObserver(session);
        }

        #region Controls

        /// <summary>
        /// WDSearchParam to find the tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public WDSearchParam UITitleParam => new WDSearchParam(WDLocators.AutomationId, "TitleYellow");

        /// <summary>
        /// WDSearchParam to find a control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public WDSearchParam UIBtnBackParam => new WDSearchParam(WDLocators.AutomationId, "BtnBack");

        /// <summary>
        /// WDSearchParam to find a control to open the previous page.
        /// </summary>
        [Aliases("button to open menu page")]
        public WDSearchParam UIBtnOpenMenuPageParam => new WDSearchParam(WDLocators.AutomationId, "BtnOpenMenuView");

        #endregion Controls

        #region Methods

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
        public override Dictionary<INavigable, Action> GetActionToNext()
        {
            return new Dictionary<INavigable, Action>()
            {
                { session.PomMenu, () => ActionToOpenMenuPage() }, // Resolve two actions opening the same page.

                // Resolve one action can open many pages (3 when conting ViewMenu).
                { session.PomBlue, () => ResolveBackBtnClick(this) },
                { session.PomRed, () => ResolveBackBtnClick(this) },
            };
        }

        /// <summary>
        /// Open the View Menu by clicking on UIBtnOpenMenuView.
        /// </summary>
        /// <returns>The ViewMenu.</returns>
        public PomMenu OpenMenuByMenuBtn()
        {
            session.WindowsDriver.Get(UIBtnOpenMenuPageParam).Click();
            return session.PomMenu;
        }

        #region Private

        /// <summary>
        /// Determines the action to open the ViewMenu by UIBtnBack depending the Navigation context.
        /// </summary>
        /// <returns>The action to open the ViewMenu.</returns>
        private void ActionToOpenMenuPage()
        {
            if (session.Previous == session.PomMenu)
            {
                session.WindowsDriver.Get(UIBtnBackParam).Click();
            }
            else
            {
                session.WindowsDriver.Get(UIBtnOpenMenuPageParam).Click();
            }
        }

        /// <summary>
        /// Resolve the navigation when the UIBackBtn is clicked.
        /// </summary>
        /// <param name="source">The source.</param>
        private void ResolveBackBtnClick(INavigable source)
        {
            List<INavigable> alternatives = new List<INavigable>()
            {
                session.PomBlue,
                session.PomRed,
                session.PomMenu
            };

            IOnActionAlternatives onActionAlternatives = new OnActionAlternatives(
                () => session.WindowsDriver.Get(UIBtnBackParam).Click(),
                alternatives);

            session.Resolve(source, onActionAlternatives);
        }

        #endregion Private

        #endregion Methods
    }
}