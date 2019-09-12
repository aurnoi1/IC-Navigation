using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Exceptions
{
    public class UnregistredNodeException : Exception
    {
        public UnregistredNodeException()
        {
        }

        public UnregistredNodeException(Type nodeType) : base($"Unregistred Node: \"{nodeType.FullName}\".")
        {
        }
    }
}
