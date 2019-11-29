using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Interfaces
{
    public interface IBrowser
    {
        IMap Map { get; }
        INavigator Navigator { get; }
    }
}
