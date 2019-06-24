using AlarmClockAccessibility.Interfaces;
using AlarmClockAccessibility.Interfaces.Pages;
using AlarmClockAccessibility.Pages;
using IC.Navigation;
using IC.Navigation.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AlarmClockAccessibility
{
    public class AlarmClockSession : NavigatorSession, IAlarmClockSession
    {
        private bool disposed = false;

        public AlarmClockSession(WindowsDriver<WindowsElement> windowsDriver, uint thinkTime)
        {
            ThinkTime = thinkTime;
            WindowsDriver = windowsDriver;
            Graph = new Graph(GetNodesByReflection(Assembly.GetExecutingAssembly()));
            EntryPoints = new HashSet<INavigable>() { PageAlarm };
        }

        #region Pages

        public IPageAlarm PageAlarm => new PageAlarm(this);

        #endregion Pages

        /// <summary>
        /// The WindowsDriver used to connect to the application.
        /// </summary>
        public WindowsDriver<WindowsElement> WindowsDriver { get; private set; }

        /// <summary>
        /// Get the Graph containing the INavigables.
        /// </summary>
        public override IGraph Graph { get; }

        /// <summary>
        /// Dispose this Instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                WindowsDriver?.Dispose();
            }

            disposed = true;
        }
    }
}