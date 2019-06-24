using AlarmClockAccessibility.Interfaces;
using AlarmClockAccessibility.Interfaces.Pages;
using AlarmClockAccessibility.Pages;
using IC.Navigation;
using IC.Navigation.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AlarmClockAccessibility
{
    public class AlarmClockSession : Navigator, IAlarmClockSession
    {
        private bool disposed = false;
        private HashSet<INavigable> navigables;

        public AlarmClockSession(WindowsDriver<WindowsElement> windowsDriver, uint thinkTime)
        {
            ThinkTime = thinkTime;
            WindowsDriver = windowsDriver;
            Graph = new Graph(Nodes);
            EntryPoints = new HashSet<INavigable>() { PageAlarm };
        }


        /// <summary>
        /// All the INavigable nodes contains in the Graph of this Navigable.
        /// </summary>
        private HashSet<INavigable> Nodes
        {
            get
            {
                if (navigables == null)
                {
                    navigables = new HashSet<INavigable>();
                    var iNavigables = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(x => typeof(INavigable).IsAssignableFrom(x) && !x.IsInterface)
                        .ToList();

                    foreach (var iNavigable in iNavigables)
                    {
                        var instance = Activator.CreateInstance(iNavigable, this) as INavigable;
                        navigables.Add(instance);
                    }
                }

                return navigables;
            }

            set
            {
                navigables = value;
            }
        }

        #region Pages

        public IPageAlarm PageAlarm => new PageAlarm(this);

        #endregion Pages

        public WindowsDriver<WindowsElement> WindowsDriver { get; private set; }

        public HashSet<INavigable> EntryPoints { get; private set; }

        public INavigable EntryPoint => Historic.FirstOrDefault();

        public uint ThinkTime { get; set; }

        public override IGraph Graph { get; }

        public TimeSpan AdjustTimeout(TimeSpan timeout)
        {
            var adjTimeout = TimeSpan.FromTicks(timeout.Ticks * ThinkTime);
            return adjTimeout;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public INavigable WaitForEntryPoints()
        {
            INavigable entryPoint = null;
            Parallel.ForEach(EntryPoints, (iNavigable, state) =>
            {
                if (!state.IsStopped && iNavigable.WaitForExists())
                {
                    entryPoint = iNavigable;
                    state.Stop();
                }
            });

            return entryPoint;
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