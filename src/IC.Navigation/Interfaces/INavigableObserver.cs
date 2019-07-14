namespace IC.Navigation.Interfaces
{
    /// <summary>
    /// Defines an observer of INavigable.
    /// </summary>
    public interface INavigableObserver
    {
        /// <summary>
        /// Update the observer with this INavigable.
        /// </summary>
        /// <param name="navigable">The INavigable.</param>
        /// <param name="args">The INavigableEventArgs.</param>
        void Update(INavigable navigable, INavigableEventArgs args);
    }
}