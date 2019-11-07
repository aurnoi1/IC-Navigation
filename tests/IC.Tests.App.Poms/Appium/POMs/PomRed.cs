using IC.Navigation;
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
    [Aliases("red page")]
    public class PomRed<R> : PomBase<R> where R : IHasSessionId, IFindsByFluentSelector<IWebElement>
    {
        public PomRed(IAppBrowser<R> session) : base(session)
        {
        }

        #region Controls

        /// <summary>
        /// WDSearchProperties to find the tile of this page.
        /// </summary>
        [Aliases("title")] // explicitly same than other pages for test.
        public SearchProperties<IWebElement> UITitle => new SearchProperties<IWebElement>(WDLocators.AutomationId, "TitleRed", session.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public SearchProperties<IWebElement> UIBtnBack => new SearchProperties<IWebElement>(WDLocators.AutomationId, "BtnBack", session.RemoteDriver);

        /// <summary>
        /// WDSearchProperties to find a control to open the yellow page.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public SearchProperties<IWebElement> UIBtnOpenYellowPage => new SearchProperties<IWebElement>(WDLocators.AutomationId, "BtnOpenYellowView", session.RemoteDriver);

        #endregion Controls

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
                { session.PomMenu, (ct) => UIBtnBack.Find(ct).Click() },
                { session.PomYellow, (ct) => UIBtnOpenYellowPage.Find(ct).Click() },
            };
        }
    }
}