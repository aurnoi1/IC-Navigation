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

        ///// <summary>
        ///// Executes the action passed in parameter.
        ///// </summary>
        ///// <param name="action">The action to execute.</param>
        ///// <returns>The current INavigable.</returns>
        //public INavigable Do(Action action) => session.Do(this, action);

        ///// <summary>
        ///// Executes the Function passed in parameter.
        ///// </summary>
        ///// <param name="function">The Function to execute.</param>
        ///// <returns>The expected INavigable returns by the Function.</returns>
        //public INavigable Do<T>(Func<INavigable> function) where T : INavigable => session.Do<T>(this, function);

        ///// <summary>
        ///// Find the shortest path of navigation to go to the destination INavigable,
        ///// then performs actions through other INavigables.
        ///// </summary>
        ///// <param name="destination">The destination.</param>
        //public INavigable GoTo(INavigable destination) => session.GoTo(this, destination);

        ///// <summary>
        ///// Performs action to step to the next INavigable.
        ///// The next INavigable must be consecutive to the current INavigable.
        ///// </summary>
        ///// <param name="destination">The opened INavigable.</param>
        //public INavigable StepToNext(INavigable destination) => session.StepToNext(GetActionToNext(), destination);

        ///// <summary>
        ///// Compares the Type name of this INavigable to another one.
        ///// </summary>
        ///// <param name="other">The other INavigable.</param>
        ///// <returns><c>true</c> if same, otherwise <c>false</c>.</returns>
        //public bool CompareTypeName(INavigable other) => session.CompareTypeNames(this, other);

        ///// <summary>
        ///// Go to the previous INavigable.
        ///// </summary>
        ///// <returns>The previous INavigable.</returns>
        //public INavigable Back() => session.Back();

        /// <summary>
        /// The navigation session.
        /// </summary>
        ISession INavigable.Session => session;
    }
}