using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Interfaces
{
    public interface INavigableObservable
    {
        WeakReference<INavigableObserver> RegisterObserver(INavigableObserver observer);
        void UnregisterObserver(WeakReference<INavigableObserver> weakObserver);
        void NotifyObservers(INavigable navigable, INavigableEventArgs args);
    }
}
