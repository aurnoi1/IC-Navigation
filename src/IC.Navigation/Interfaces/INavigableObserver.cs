using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Interfaces
{
    public interface INavigableObserver
    {
        void UpdateHistoric(INavigable navigable);
    }
}
