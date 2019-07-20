using System;
using System.Collections.Generic;
using System.Reflection;

namespace IC.Navigation.Interfaces
{
    public interface INavigatorSession : ISession
    {
        T GetINavigableInstance<T>(Type type) where T : INavigable;

        HashSet<INavigable> GetNodesByReflection(Assembly assembly);
    }
}