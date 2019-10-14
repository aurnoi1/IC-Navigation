using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Exceptions
{
    public class UninitializedGraphException : Exception
    {
        public UninitializedGraphException() : base($"Graph is uninitialized.")
        {
        }
    }
}
