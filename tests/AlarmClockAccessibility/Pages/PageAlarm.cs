using AlarmClockAccessibility.Interfaces;
using AlarmClockAccessibility.Interfaces.Pages;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;

namespace AlarmClockAccessibility.Pages
{
    public class PageAlarm : IPageAlarm
    {
        private IAlarmClockSession session;

        public PageAlarm(in IAlarmClockSession alarmClockSession)
        {
            session = alarmClockSession;
        }

        public ISession Session => session;

        public Dictionary<INavigable, Action> GetActionToNext()
        {
            throw new NotImplementedException();
        }

        public bool WaitForExists()
        {
            bool exists = UIAlarmListView != null;
            session.SetLast(this, exists);
            return exists;
        }

        #region Controls

        public WindowsElement UIAlarmListView => session.WindowsDriver.FindElementByAccessibilityId(
            "AlarmListView",
            Session.AdjustTimeout(TimeSpan.FromSeconds(3)));

        #endregion Controls
    }
}