using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;

namespace IC.Tests.App.UIAccessibility.Appium.POMs
{
    [UIArtifact("menu view")]
    public class PomMenu : INavigable
    {
        private readonly IFacade session;
        private readonly List<WeakReference<INavigableObserver>> observers = new List<WeakReference<INavigableObserver>>();

        public PomMenu(in IFacade session)
        {
            this.session = session;
            RegisterObserver(session);
        }

        #region Controls

        /// <summary>
        /// A control NOT IMPLEMENTED only use for negative test.
        /// </summary>
        [UIArtifact("not implemented")]
        public WindowsElement UIBtnNotImplemented => session.WindowsDriver.FindElementByAccessibilityId(
            "NotImplemented",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// The tile of this view.
        /// </summary>
        [UIArtifact("title")] // explicitly same than other views for test.
        public WindowsElement UITitle => session.WindowsDriver.FindElementByAccessibilityId(
            "TitleMenu",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the BlueView.
        /// </summary>
        [UIArtifact("button to open the blue view")]
        public WindowsElement UIBtnOpenBlueView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenBlueView",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the RedView.
        /// </summary>
        [UIArtifact("button to open the red view")]
        public WindowsElement UIBtnOpenRedView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenRedView",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control to open the RedView.
        /// </summary>
        [UIArtifact("button to open the yellow view")]
        public WindowsElement UIBtnOpenYellowView => session.WindowsDriver.FindElementByAccessibilityId(
            "BtnOpenYellowView",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        /// <summary>
        /// A control where text can be enter.
        /// </summary>
        [UIArtifact("box where enter text")]
        public WindowsElement UITxtBoxImportantMessage => session.WindowsDriver.FindElementByAccessibilityId(
            "TxtBoxImportantMessage",
            session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        #endregion Controls

        /// <summary>
        /// Enter a text in the UITxtBoxImportantMessage.
        /// </summary>
        /// <param name="text">The text to enter.</param>
        public void EnterText(string text)
        {
            UITxtBoxImportantMessage.SendKeys(text);
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
                { session.ViewBlue, () => UIBtnOpenBlueView.Click() },
                { session.ViewRed, () => UIBtnOpenRedView.Click() },
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