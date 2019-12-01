using IC.Navigation;
using IC.Navigation.Enums;
using IC.Navigation.Extension.Appium;
using IC.Navigation.Extension.Appium.WindowsDriver;
using IC.Navigation.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Tests.App.Poms.Appium.POMs
{
    public class PomYellow<R> : PomBase<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        public PomYellow(Map<R> map, ILog log, CancellationToken globalCancellationToken) : base(map, log, globalCancellationToken)
        {
        }

        #region Controls

        /// <summary>
        /// WDSearchProperties to find the tile of this page.
        /// </summary>
        public SearchProperties<IWebElement> UITitle => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "TitleYellow", map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the previous page.
        /// </summary>
        public SearchProperties<IWebElement> UIBtnBack => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "BtnBack", map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the previous page.
        /// </summary>
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
                { map.PomBlue, (ct) => UIBtnBack.Find(ct).Click() },
                { map.PomRed, (ct) => UIBtnBack.Find(ct).Click() },
            };
        }

        public override HashSet<DynamicNeighbor> GetDynamicNeighbors()
        {
            var dynamicPaths = new HashSet<DynamicNeighbor>();
            var alternatives = new HashSet<INavigable>() { map.PomRed, map.PomBlue, map.PomMenu };
            dynamicPaths.Add(new DynamicNeighbor(this, alternatives));
            return dynamicPaths;
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
                    globalCancellationToken,
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
            if (log.Previous == map.PomMenu)
            {
                UIBtnBack.Find(ct).Click();
            }
            else
            {
                UIBtnOpenMenuPage.Find(ct).Click();
            }
        }

        #endregion Private

        #endregion Methods
    }
}