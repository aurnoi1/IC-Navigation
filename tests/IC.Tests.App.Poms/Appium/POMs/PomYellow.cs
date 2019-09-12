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
        public SearchParam UITitleParam => new SearchParam(WDLocators.AutomationId, "TitleYellow");

        /// <summary>
        /// WDSearchParam to find a control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public SearchParam UIBtnBackParam => new SearchParam(WDLocators.AutomationId, "BtnBack");

        /// <summary>
        /// WDSearchParam to find a control to open the previous page.
        /// </summary>
        [Aliases("button to open menu page")]
        public SearchParam UIBtnOpenMenuPageParam => new SearchParam(WDLocators.AutomationId, "BtnOpenMenuView");

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
        public override Dictionary<INavigable, Action<CancellationToken>> GetActionToNext()
        {
            return new Dictionary<INavigable, Action<CancellationToken>>()
            {
                { session.PomMenu, (ct) => ActionToOpenMenuPage(ct) }, // Resolve two actions opening the same page.

                // Resolve one action can open many pages (3 when counting ViewMenu).
                { session.PomBlue, (ct) => ResolveBackBtnClick(this, ct) },
                { session.PomRed, (ct) => ResolveBackBtnClick(this, ct) },
            };
        }

        /// <summary>
        /// Open the View Menu by clicking on UIBtnOpenMenuView.
        /// </summary>
        /// <param name="ct">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The ViewMenu.</returns>
        public PomMenu OpenMenuByMenuBtn(CancellationToken ct)
        {
            session.WindowsDriver.Get(UIBtnOpenMenuPageParam, ct)?.Click();
            return session.PomMenu;
        }

        #region Private

        /// <summary>
        /// Determines the action to open the ViewMenu by UIBtnBack depending the Navigation context.
        /// </summary>
        /// <param name="ct">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The action to open the ViewMenu.</returns>
        private void ActionToOpenMenuPage(CancellationToken ct)
        {
            if (session.Previous == session.PomMenu)
            {
                session.WindowsDriver.Get(UIBtnBackParam, ct)?.Click();
            }
            else
            {
                session.WindowsDriver.Get(UIBtnOpenMenuPageParam, ct)?.Click();
            }
        }

        /// <summary>
        /// Resolve the navigation when the UIBackBtn is clicked.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="ct">The CancellationToken to interrupt the task as soon as possible.</param>
        private void ResolveBackBtnClick(INavigable source, CancellationToken ct)
        {
            List<INavigable> alternatives = new List<INavigable>()
            {
                session.PomBlue,
                session.PomRed,
                session.PomMenu
            };

            IOnActionAlternatives onActionAlternatives = new OnActionAlternatives(
                () => session.WindowsDriver.Get(UIBtnBackParam)?.Click(),
                alternatives);

            session.Resolve(source, onActionAlternatives, ct);
        }

        #endregion Private

        #endregion Methods
    }
}