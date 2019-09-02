namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines an observer of INavigable.
    /// </summary>
    public interface INavigableObserver
    {
        /// <summary>
        /// Update the observer with this Navigable.
        /// </summary>
        /// <param name="navigable">The Navigable.</param>
        /// <param name="status">The NavigableStatus.</param>
        void Update(INavigable navigable, INavigableStatus status);
    }
}