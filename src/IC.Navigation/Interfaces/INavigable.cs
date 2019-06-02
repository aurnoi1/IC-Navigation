using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines a INavigable.
    /// </summary>
    public interface INavigable
    {
        /// <summary>
        /// The navigation session.
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// Compares the Type name of this INavigable to another one.
        /// </summary>
        /// <param name="other">The other INavigable.</param>
        /// <returns><c>true</c> if same, otherwise <c>false</c>.</returns>
        bool CompareTypeName(INavigable other);

        /// <summary>
        /// Find the shortest path of navigation to go to the destination INavigable,
        /// then performs actions through other INavigables.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <returns>The destination.</returns>
        INavigable GoTo(INavigable destination);

        /// <summary>
        /// Performs action to go to the next INavigable in the resolve path.
        /// The next INavigable must be a directe or rebased consecutive to the current INavigable.
        /// </summary>
        /// <param name="destination">The opened INavigable.</param>
        INavigable StepToNext(INavigable destination);

        /// <summary>
        /// Go to the previous INavigable.
        /// </summary>
        /// <returns>The previous INavigable.</returns>
        INavigable Back();

        /// <summary>
        /// Waits for the current INavigable to be fully loaded.
        /// </summary>
        bool WaitForExists();

        /// <summary>
        /// Executes the action passed in parameter.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The current INavigable.</returns>
        INavigable Do(Action action);

        /// <summary>
        /// Executes the Function passed in parameter.
        /// </summary>
        /// <param name="function">The Function to execute.</param>
        /// <returns>The expected INavigable returns by the Function.</returns>
        INavigable Do<T>(Func<INavigable> function) where T : INavigable;

        /// <summary>
        /// Gets a Dictionary of action to go to the next INavigable.
        /// </summary>
        /// <returns>A Dictionary of action to go to the next INavigable.</returns>
        Dictionary<INavigable, Action> GetActionToNext();
    }
}
