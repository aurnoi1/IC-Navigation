using IC.Navigation;
using IC.Navigation.Enums;
using IC.Navigation.Extension.Appium;
using IC.Navigation.Extension.Appium.WindowsDriver;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Tests.App.Poms.Appium.POMs
{
    [Aliases("yellow page")]
    public class PomYellow<R> : PomBase<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        public PomYellow(Map<R> map) : base(map)
        {
        }

        #region Controls

        /// <summary>
        /// WDSearchProperties to find the tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public SearchProperties<IWebElement> UITitle => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "TitleYellow", map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public SearchProperties<IWebElement> UIBtnBack => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "BtnBack", map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the previous page.
        /// </summary>
        [Aliases("button to open menu page")]
        public SearchProperties<IWebElement> UIBtnOpenMenuPage => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "BtnOpenMenuView", map.RemoteDriver);

        #endregion Controls

        #region Methods

        /// <summary>
        /// Notify observers of a specific State's value.
        /// </summary>
        /// <typeparam name="T">The State's value type.</typeparam>
        /// <param name="stateName">The state name.</param>
        /// <returns>The State.</returns>
        public override IState<T> PublishState<T>(StatesNames stateName)
        {
            bool isDisplayed = (UITitle.Get() != null);
            var genericIsDisplayed = (T)Convert.ChangeType(isDisplayed, typeof(T));
            State<T> state = stateName switch
            {
                StatesNames.Exist => new State<T>(this, StatesNames.Exist, genericIsDisplayed),
                StatesNames.Ready => new State<T>(this, StatesNames.Ready, genericIsDisplayed),
                _ => throw new ArgumentException($"Undefined {nameof(StatesNames)}: {stateName}."),
            };

            NotifyObservers(state);
            return state;
        }

        /// <summary>
        /// Gets a Dictionary of action to go to the next INavigable.
        /// </summary>
        /// <returns>A Dictionary of action to go to the next INavigable.</returns>
        public override Dictionary<INavigable, Action<CancellationToken>> GetActionToNext()
        {
            return new Dictionary<INavigable, Action<CancellationToken>>()
            {
                { map.PomMenu, (ct) => ActionToOpenMenuPage(ct) }, // Resolve two actions opening the same page.

                // Resolve one action can open many pages (3 when counting ViewMenu).
                { map.PomBlue, (ct) => ResolveBackBtnClick(this, ct) },
                { map.PomRed, (ct) => ResolveBackBtnClick(this, ct) },
            };
        }

        /// <summary>
        /// Open the Menu page by clicking on UIBtnOpenMenuPage.
        /// </summary>
        /// <param name="timeout">The timeout to interrupt the task as soon as possible in concurrence
        /// of <see cref="AppBrowser.GlobalCancellationToken"/>.</param>
        /// <returns>The PomMenu.</returns>
        public PomMenu<R> OpenMenuByMenuBtn(TimeSpan timeout)
        {
            CancellationTokenSource localCts = default;
            try
            {
                localCts = new CancellationTokenSource(timeout);
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    map.GlobalCancellationToken,
                    localCts.Token);

                UIBtnOpenMenuPage.Find(linkedCts.Token).Click();
                return map.PomMenu;
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
            if (map.Log.Previous == map.PomMenu)
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
                map.PomBlue,
                map.PomRed,
                map.PomMenu
            };

            IOnActionAlternatives onActionAlternatives = new OnActionAlternatives(
                (x) => UIBtnBack.Find(x).Click(),
                alternatives);

            map.Resolve(source, onActionAlternatives, ct);
        }

        #endregion Private

        #endregion Methods
    }
}