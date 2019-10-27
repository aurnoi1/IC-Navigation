using IC.Navigation.Interfaces;
using System;

namespace IC.Navigation.Exceptions
{
    public class UnexpectedNavigableException : Exception
    {
        public UnexpectedNavigableException()
        {
        }

        public UnexpectedNavigableException(INavigable unexpected)
            : base($"The Navigable of type \"{unexpected.GetType().FullName}\" was not expected.")
        {
        }

        public UnexpectedNavigableException(Type expected, INavigable unexpected)
            : base($"The Navigable of type \"{expected.FullName}\" was expected but found \"{unexpected.GetType().FullName}\" instead.")
        {
        }
    }
}