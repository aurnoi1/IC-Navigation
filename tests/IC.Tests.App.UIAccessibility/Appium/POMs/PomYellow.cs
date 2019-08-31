using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;

namespace IC.Tests.App.UIAccessibility.Appium.POMs
{
    [UIArtifact("yellow view")]
    public class PomYellow : INavigable
    {
        private readonly IFacade session;
        private readonly List<WeakReference<INavigableObserver>> observers = new List<WeakReference<INavigableObserver>>();

        public PomYellow(in IFacade session)
        {
            this.session = session;
            RegisterObserver(session);
        }

        #region Controls

        /// <summary>
        /// The tile of this view.
        /// </summary>
        [UIArtifact("title")] // explicitly same than other views for test.
        public WindowsElement UITitle => session.WindowsDriver.FindElementByAccessibilityId(
            "TitleYellow",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the previous page.
        /// </summary>
        [UIArtifact("button to go back to the previous view")]
        public WindowsElement UIBtnBack => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnBack",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the previous page.
        /// </summary>
        [UIArtifact("button to open menu view")]
        public WindowsElement UIBtnOpenMenuView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenMenuView",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        #endregion Controls

        /// <summary>
        /// Open the View Menu by clicking on UIBtnOpenMenuView.
        /// </summary>
        /// <returns>The ViewMenu.</returns>
        public PomMenu OpenMenuByMenuBtn()
        {
            UIBtnOpenMenuView.Click();
            return session.ViewMenu;
        }

        /// <summary>
        /// Determines the action to open the ViewMenu by UIBtnBack depending the Navigation context.
        /// </summary>
        /// <returns>The action to open the ViewMenu.</returns>
        private void ActionToOpenViewMenu()
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
        private void ResolveBackBtnClick(INavigable source)
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

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        public bool PublishExistsStatus()
        {
            bool isDisplayed = UITitle != null;
            INavigableEventArgs args = new NavigableEventArgs() { Exists = isDisplayed };
            NotifyObservers(args);
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
                { session.ViewMenu, () => ActionToOpenViewMenu() }, // Resolve two actions opening the same view.

                // Resolve one action can open many views (3 when conting ViewMenu).
                { session.ViewBlue, () => ResolveBackBtnClick(this) },
                { session.ViewRed, () => ResolveBackBtnClick(this) },
            };
        }

        /// <summary>
        /// Register the INavigableObserver as a WeakReference.
        /// </summary>
        /// <param name="observer">The INavigableObserver.</param>
        /// <returns>The INavigableObserver as a WeakReference.</returns>
        public WeakReference<INavigableObserver> RegisterObserver(INavigableObserver observer)
        {
            var weakObserver = new WeakReference<INavigableObserver>(observer);
            observers.Add(weakObserver);
            return weakObserver;
        }

        /// <summary>
        /// Unregister the INavigableObserver.
        /// </summary>
        /// <param name="weakObserver">The INavigableObserver as a WeakReference.</param>
        public void UnregisterObserver(WeakReference<INavigableObserver> weakObserver)
        {
            observers.Remove(weakObserver);
        }

        /// <summary>
        /// Notify all observers.
        /// </summary>
        /// <param name="args">The INavigableEventArgs.</param>
        public void NotifyObservers(INavigableEventArgs args)
        {
            observers.ForEach(x =>
            {
                x.TryGetTarget(out INavigableObserver obs);
                if (obs == null)
                {
                    UnregisterObserver(x);
                }
                else
                {
                    obs.Update(this, args);
                }
            });
        }

        /// <summary>
        /// The navigation session.
        /// </summary>
        ISession INavigable.Session => session;
    }
}