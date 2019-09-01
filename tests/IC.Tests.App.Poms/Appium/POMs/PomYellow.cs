using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
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
        /// The tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public WindowsElement UITitle => session.WindowsDriver.FindElementByAccessibilityId(
            "TitleYellow",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public WindowsElement UIBtnBack => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnBack",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the previous page.
        /// </summary>
        [Aliases("button to open menu page")]
        public WindowsElement UIBtnOpenMenuPage => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenMenuView",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        #endregion Controls

        #region Methods

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
            UIBtnOpenMenuPage.Click();
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
                UIBtnBack.Click();
            }
            else
            {
                UIBtnOpenMenuPage.Click();
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
                () => UIBtnBack.Click(),
                alternatives);

            session.Resolve(source, onActionAlternatives);
        }

        #endregion Private

        #endregion Methods
    }
}