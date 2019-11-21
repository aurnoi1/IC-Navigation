using IC.Navigation.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Interfaces
{
    public interface IState<T>
    {
        StatesNames Name { get; }
        T Value { get; }
    }
}
