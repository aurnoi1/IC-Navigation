using IC.Navigation;
using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewDefinitions;
using IC.Tests.App.UIAccessibility.Appium.ViewNavigables;
using System;
using System.Collections.Generic;

namespace IC.Tests.App.UIAccessibility.Appium.ViewFeatures
{
    public abstract class ViewFeatYellow : ViewDefYellow, IViewFeatYellow
    {
        public ViewFeatYellow(in Interfaces.IMySession session) : base(session) => this.session = session;

        private Interfaces.IMySession session;

        #region Methods

        #region Public

        /// <summary>
        /// Open the View Menu by clicking on UIBtnOpenMenuView.
        /// </summary>
        /// <returns>The ViewMenu.</returns>
        public IViewMenu OpenViewMenuByMenuBtn()
        {
            UIBtnOpenMenuView.Click();
            return session.ViewMenu;
        }

        #region Protected

        /// <summary>
        /// Determines the action to open the ViewMenu by UIBtnBack depending the Navigation context.
        /// </summary>
        /// <returns>The action to open the ViewMenu.</returns>
        protected private void ActionToOpenViewMenu()
        {
            if (session.Previous == session.ViewMenu)
            {
                UIBtnBack.Click();
            }
            else
            {
                UIBtnOpenMenuView.Click();
            }
        }

        /// <summary>
        /// Resolve the navigation when the UIBackBtn is clicked.
        /// </summary>
        /// <param name="source">The source.</param>
        protected private void ResolveBackBtnClick(INavigable source)
        {
            List<INavigable> alternatives = new List<INavigable>()
            {
                session.ViewBlue,
                session.ViewRed,
                session.ViewMenu
            };

            IOnActionAlternatives onActionAlternatives = new OnActionAlternatives(
                () => UIBtnBack.Click(),
                alternatives);

            session.Resolve(source, onActionAlternatives);
        }

        #endregion Protected

        #endregion Public

        #endregion Methods
    }
}