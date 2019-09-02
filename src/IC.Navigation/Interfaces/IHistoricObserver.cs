using System.Collections.Generic;

namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines an observer of INavigable.
    /// </summary>
    public interface IHistoricObserver
    {
        /// <summary>
        /// Update the observer with this historic.
        /// </summary>
        /// <param name="historic">The updated historic.</param>
        void Update(List<INavigable> historic);
    }
}