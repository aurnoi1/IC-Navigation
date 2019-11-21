using IC.Navigation;
using IC.Navigation.Enums;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Appium.WindowsDriver;
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
    [Aliases("menu page")]
    public class PomMenu<R> : PomBase<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>

    {
        public PomMenu(IAppBrowser<R> session) : base(session)
        {
        }

        #region Controls

        /// <summary>
        /// WDSearchProperties to find a control NOT IMPLEMENTED only use for negative test.
        /// </summary>
        [Aliases("not implemented")]
        public SearchProperties<IWebElement> UIBtnNotImplemented => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "NotImplemented", session.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find the tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public SearchProperties<IWebElement> UITitle => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "TitleMenu", session.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the BlueView.
        /// </summary>
        [Aliases("button to open the blue page")]
        public SearchProperties<IWebElement> UIBtnOpenBluePage => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "BtnOpenBlueView", session.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the RedView.
        /// </summary>
        [Aliases("button to open the red page")]
        public SearchProperties<IWebElement> UIBtnOpenRedPage => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "BtnOpenRedView", session.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the RedView.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public SearchProperties<IWebElement> UIBtnOpenYellowPage => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "BtnOpenYellowView", session.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control where text can be enter.
        /// </summary>
        [Aliases("box where enter text")]
        public SearchProperties<IWebElement> UITxtBoxImportantMessage => new SearchProperties<IWebElement>(WindowDriverLocators.AutomationId, "TxtBoxImportantMessage", session.RemoteDriver);

        #endregion Controls

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        public override INavigableStatus PublishStatus()
        {
            bool isDisplayed = PublishState<bool>(StatesNames.Exist).Value;
            NavigableStatus status = new NavigableStatus(this)
            {
                Exist = new State<bool>(this, StatesNames.Exist, isDisplayed),
                Ready = new State<bool>(this, StatesNames.Ready, isDisplayed)
            };

            NotifyObservers(status);
            return status;
        }

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
                { session.PomBlue, (ct) => UIBtnOpenBluePage.Find(ct).Click() },
                { session.PomRed, (ct) => UIBtnOpenRedPage.Find(ct).Click() },
                { session.PomYellow, (ct) => UIBtnOpenYellowPage.Find(ct).Click() },
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