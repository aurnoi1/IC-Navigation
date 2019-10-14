using IC.Navigation;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.Interfaces;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Tests.App.Poms.Appium.POMs
{
    [Aliases("blue page")]
    public class PomBlue : PomBase
    {
        public PomBlue(in IFacade session) : base(session)
        {
            RegisterObserver(session);
        }

        #region Controls

        /// <summary>
        /// WDSearchParam to find the title of this page.
        /// </summary>
        [Aliases("title")]
        public SearchParam<WindowsElement> UILblTitle => new SearchParam<WindowsElement>(WDLocators.AutomationId, "TitleBlue", session.WindowsDriver);

        /// <summary>
        /// WDSearchParam to find a control to open the previous page.
        /// </summary>
        [Aliases("button to go back to the previous page")]
        public SearchParam<WindowsElement> UIBtnBack => new SearchParam<WindowsElement>(WDLocators.AutomationId, "BtnBack", session.WindowsDriver);

        /// <summary>
        /// WDSearchParam to find a control to open the yellow page.
        /// </summary>
        [Aliases("button to open the yellow page")]
        public SearchParam<WindowsElement> BtnOpenYellowView => new SearchParam<WindowsElement>(WDLocators.AutomationId, "BtnOpenYellowView", session.WindowsDriver);

        #endregion Controls

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        public override INavigableStatus PublishStatus()
        {
            bool isDisplayed = session.WindowsDriver.Get(UILblTitle) != null;
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
                { session.PomMenu, (ct) => session.WindowsDriver.Find(UIBtnBack, ct).Click() },
                { session.PomYellow, (ct) => session.WindowsDriver.Find(BtnOpenYellowView, ct).Click() },
            };
        }
    }
}