﻿using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Tests.App.Poms.Appium.POMs
{
    [Aliases("yellow page")]
    public class PomYellow<R> : PomBase<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        public PomYellow(IFacade<R> session) : base(session)
        {
            RegisterObserver(session);
        }

        #region Controls

        /// <summary>
        /// WDSearchProperties to find the tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public SearchProperties<IWebElement> UITitle => new SearchProperties<IWebElement>(WDLocators.AutomationId, "TitleYellow", session.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public SearchProperties<IWebElement> UIBtnBack => new SearchProperties<IWebElement>(WDLocators.AutomationId, "BtnBack", session.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the previous page.
        /// </summary>
        [Aliases("button to open menu page")]
        public SearchProperties<IWebElement> UIBtnOpenMenuPage => new SearchProperties<IWebElement>(WDLocators.AutomationId, "BtnOpenMenuView", session.RemoteDriver);

        #endregion Controls

        #region Methods

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        public override INavigableStatus PublishStatus()
        {
            bool isDisplayed = UITitle.Get() != null;
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
        /// Open the Menu page by clicking on UIBtnOpenMenuPage.
        /// </summary>
        /// <param name="timeout">The timeout to interrupt the task as soon as possible in concurrence
        /// of <see cref="Facade.GlobalCancellationToken"/>.</param>
        /// <returns>The PomMenu.</returns>
        public PomMenu<R> OpenMenuByMenuBtn(TimeSpan timeout)
        {
            CancellationTokenSource localCts = default;
            try
            {
                localCts = new CancellationTokenSource(timeout);
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    session.GlobalCancellationToken, 
                    localCts.Token);

                UIBtnOpenMenuPage.Find(linkedCts.Token).Click();
                return session.PomMenu;
            }
            catch (Exception)
            {
                localCts?.Dispose();
                throw;
            }
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
                UIBtnBack.Find(ct).Click();
            }
            else
            {
                UIBtnOpenMenuPage.Find(ct).Click();
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
                (x) => UIBtnBack.Find(x).Click(),
                alternatives);

            session.Resolve(source, onActionAlternatives, ct);
        }

        #endregion Private

        #endregion Methods
    }
}