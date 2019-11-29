using IC.Navigation.Enums;
using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IC.Navigation
{
    public class Log : ILog
    {

        /// <summary>
        /// Lock to queue historic entries.
        /// </summary>
        private readonly object historicLock = new object();

        public virtual INavigable Previous
        {
            get
            {
                lock (historicLock)
                {
                    return Historic.Count > 1 ? Historic[Historic.Count - 2] : null;
                }
            }
        }

        public INavigable Last
        {
            get
            {
                lock (historicLock)
                {
                    return Historic.LastOrDefault();
                }
            }

            private set
            {
                lock (historicLock)
                {
                    if (value != null && value != Historic.LastOrDefault())
                    {
                        Historic.Add(value);
                    }
                }
            }
        }

        public List<INavigable> Historic { get; private set; } = new List<INavigable>();

        public void Update(INavigableStatus status)
        {
            SetLast(status.Exist);
        }

        public void Update<T>(IState<T> state)
        {
            if (state.Name == StatesNames.Exist)
            {
                Last = state.Navigable;
            }
        }


        /// <summary>
        /// Set the last known INavigable is exists.
        /// </summary>
        /// <param name="status">The NavigableStatus of the last INavigable.</param>
        /// <returns><c>true</c> if the INavigable exists, otherwise <c>false</c>.</returns>
        private void SetLast(IState<bool> state)
        {
            if (state.Name == StatesNames.Exist)
            {
                Last = state.Navigable;
            }
        }

    }
}
