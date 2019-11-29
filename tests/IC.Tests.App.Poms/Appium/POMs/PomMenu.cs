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
    public class PomMenu<R> : PomBase<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>

    {
        public PomMenu(Map<R> map, ILog log, CancellationToken globalCancellationToken) : base(map, log, globalCancellationToken)
        {
        }

        #region Controls

        /// <summary>
        /// WDSearchProperties to find a control NOT IMPLEMENTED only use for negative test.
        /// </summary>
        public SearchProperties<IWebElement> UIBtnNotImplemented => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "NotImplemented", map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find the tile of this page.
        /// </summary>
        public SearchProperties<IWebElement> UITitle => new SearchProperties<IWebElement>(
            WindowDriverLocators.AutomationId,
            "TitleMenu", 
            map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the BlueView.
        /// </summary>
        public SearchProperties<IWebElement> UIBtnOpenBluePage => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "BtnOpenBlueView", map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the RedView.
        /// </summary>
        public SearchProperties<IWebElement> UIBtnOpenRedPage => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "BtnOpenRedView", map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the RedView.
        /// </summary>
        public SearchProperties<IWebElement> UIBtnOpenYellowPage => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "BtnOpenYellowView", map.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control where text can be enter.
        /// </summary>
        public SearchProperties<IWebElement> UITxtBoxImportantMessage => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "TxtBoxImportantMessage", map.RemoteDriver);

        #endregion Controls

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
                { map.PomBlue, (ct) => UIBtnOpenBluePage.Find(ct).Click() },
                { map.PomRed, (ct) => UIBtnOpenRedPage.Find(ct).Click() },
                { map.PomYellow, (ct) => UIBtnOpenYellowPage.Find(ct).Click() },
            };
        }

        /// <summary>
        /// Enter a text in the UITxtBoxImportantMessage.
        /// </summary>
        /// <param name="text">The text to enter.</param>
        public void EnterText(string text)
        {
            UITxtBoxImportantMessage.Get().SendKeys(text);
        }
    }
}