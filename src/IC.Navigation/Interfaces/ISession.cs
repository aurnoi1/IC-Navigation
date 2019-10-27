using System;
using System.Collections.Generic;
using System.Threading;

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
        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
        /// <returns>The first INavigable found, otherwise <c>null</c>.</returns>
        INavigable WaitForEntryPoints(CancellationToken cancellationToken);

        /// <summary>
        /// Wait for any EntryPoints of the navigation to exists.
        /// The amount of time to wait is defined by each INavigable.WaitForExists().
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for any EntryPoints.</param>
        /// <returns>The first INavigable found, otherwise <c>null</c>.</returns>
        /// <exception cref="TimeoutException">Throw when timeout is reached before any EntryPoint is found.</exception>
        INavigable WaitForEntryPoints(TimeSpan timeout);

        /// <summary>
        /// The INavigable EntryPoint that is found at the beginning of the navigation.
        /// </summary>
        INavigable EntryPoint { get; }
    }
}