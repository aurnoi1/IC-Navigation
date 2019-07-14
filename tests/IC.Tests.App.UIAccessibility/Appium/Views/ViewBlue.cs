﻿using IC.Navigation;
using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IC.Tests.App.UIAccessibility.Appium.ViewNavigables
{
    [UIArtefact("blue view")]
    public class ViewBlue : ViewFeatBlue, INavigable, IViewBlue
    {
        private readonly IUIAccess session;
        private readonly List<WeakReference<INavigableObserver>> observers = new List<WeakReference<INavigableObserver>>();

        public ViewBlue(in IUIAccess session) : base(session)
        {
            this.session = session;
            RegisterObserver(session);
        }

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        public bool WaitForExists()
        {
            bool isDisplayed = UITitle != null;
            if (isDisplayed) { NotifyUpdateHistoric(this); }
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

        public WeakReference<INavigableObserver> RegisterObserver(INavigableObserver observer)
        {
            var weakObserver = new WeakReference<INavigableObserver>(observer);
            observers.Add(weakObserver);
            return weakObserver;
        }

        public void UnregisterObserver(WeakReference<INavigableObserver> weakObserver)
        {
            observers.Remove(weakObserver);
        }

        /// <summary>
        /// Notify the observers.
        /// </summary>
        /// <param name="navigable"></param>
        public void NotifyUpdateHistoric(INavigable navigable)
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
                    obs.UpdateHistoric(navigable);
                }
            });
        }

        /// <summary>
        /// The navigation session.
        /// </summary>
        ISession INavigable.Session => session;
    }
}