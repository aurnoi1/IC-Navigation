using IC.Navigation;
using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewFeatures;
using System;
using System.Collections.Generic;

namespace IC.Tests.App.UIAccessibility.Appium.ViewNavigables
{
    [UIArtifact("red view")]
    public class ViewRed : ViewFeatRed, IViewRed
    {
        private readonly List<WeakReference<INavigableObserver>> observers = new List<WeakReference<INavigableObserver>>();
        private readonly IFacade session;

        public ViewRed(in IFacade session) : base(session)
        {
            this.session = session;
            RegisterObserver(session);
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
                { session.ViewMenu, () => UIBtnBack.Click() },
                { session.ViewYellow, () => UIBtnOpenYellowView.Click() },
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