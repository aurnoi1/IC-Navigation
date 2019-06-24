using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;

namespace IC.Navigation.Interfaces
{
    public interface ISession : INavigator
    {
        /// <summary>
        /// The INavigables to be expected as entry points when the application start.
        /// </summary>
        HashSet<INavigable> EntryPoints { get; }

        /// <summary>
        /// Wait for any EntryPoints of the navigation to exists.
        /// The amount of time to wait is defined by each INavigable.WaitForExists().
        /// </summary>
        /// <returns>The first INavigable found, otherwise <c>null</c>.</returns>
        INavigable WaitForEntryPoints();

        /// <summary>
        /// The INavigable EntryPoint that is found at the beginning of the navigation.
        /// </summary>
        INavigable EntryPoint { get; }

        /// <summary>
        /// Multiplicator to adjust the timeouts when waiting for UI objects.
        /// </summary>
        uint ThinkTime { get; set; }

        /// <summary>
        /// Adjust the timeout when waiting for the UI objects depending the <see cref="ThinkTime"/> value.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The adjusted timeout.</returns>
        TimeSpan AdjustTimeout(TimeSpan timeout);
    }
}