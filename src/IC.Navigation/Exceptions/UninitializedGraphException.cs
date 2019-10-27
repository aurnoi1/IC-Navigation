using System;

namespace IC.Navigation.Exceptions
{
    public class UninitializedGraphException : Exception
    {
        public UninitializedGraphException() : base($"Graph is uninitialized.")
        {
        }
    }
}