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
    public class PomBlue<R> : PomBase<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        public PomBlue(Map<R> map, ILog log, CancellationToken globalCancellationToken) : base(map, log, globalCancellationToken)
        {
        }

        #region Controls

        /// <summary>
        /// WDSearchProperties to find the title of this page.
        /// </summary>
        public SearchProperties<IWebElement> UILblTitle => new SearchProperties<IWebElement>(
            WindowDriverLocators.AutomationId,
            "TitleBlue",
            map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the previous page.
        /// </summary>
        public SearchProperties<IWebElement> UIBtnBack => new SearchProperties<IWebElement>(
            WindowDriverLocators.AutomationId,
            "BtnBack",
            map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the yellow page.
        /// </summary>
        public SearchProperties<IWebElement> BtnOpenYellowPage => new SearchProperties<IWebElement>(
            WindowDriverLocators.AutomationId,
            "BtnOpenYellowView",
            map.RemoteDriver);

        #endregion Controls

        /// <summary>
        /// Notify observers of a specific State's value.
        /// </summary>
        /// <typeparam name="T">The State's value type.</typeparam>
        /// <param name="stateName">The state name.</param>
        /// <returns>The State.</returns>
        public override IState<T> PublishState<T>(StatesNames stateName)
        {
            bool isDisplayed = (UILblTitle.Get() != null);
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
                { map.PomMenu, (ct) => UIBtnBack.Find(ct).Click() },
                { map.PomYellow, (ct) => BtnOpenYellowPage.Find(ct).Click() },
            };
        }
    }
}