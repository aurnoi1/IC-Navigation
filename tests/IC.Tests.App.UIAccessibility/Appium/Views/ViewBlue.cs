using IC.Navigation;
using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewFeatures;
using System;
using System.Collections.Generic;

namespace IC.Tests.App.UIAccessibility.Appium.ViewNavigables
{
    [UIArtefact("blue view")]
    public class ViewBlue : ViewFeatBlue, INavigable, IViewBlue
    {
        private IMySession session;

        public ViewBlue(in IMySession session) : base(session) => this.session = session;

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        public bool WaitForExists()
        {
            bool isDisplayed = UITitle != null;
            session.SetLast(this, isDisplayed);
            return isDisplayed;
        }

        /// <summary>
        /// Gets a Dictionary of action to go to the next INavigable.
        /// </summary>
        /// <returns>A Dictionary of action to go to the next INavigable.</returns>
        public Dictionary<INavigable, Action> GetActionToNext()
        {
            return new Dictionary<INavigable, Action>()
            {
                { session.ViewMenu, () => UIBtnBack.Click() },
                { session.ViewYellow, () => UIBtnOpenYellowView.Click() },
            };
        }

        /// <summary>
        /// The navigation session.
        /// </summary>
        ISession INavigable.Session => session;
    }
}