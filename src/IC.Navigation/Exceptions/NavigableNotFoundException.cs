using IC.Navigation.Interfaces;
using System;

namespace IC.Navigation.Exceptions
{
    public class NavigableNotFoundException : Exception
    {
        public NavigableNotFoundException()
        {
        }

        public NavigableNotFoundException(string definition, INavigable navigable)
            : base($"The {definition} \"{navigable.GetType().FullName}\" was not found.")
        {
        }

        public NavigableNotFoundException(string definition, INavigable navigable, string supplementalInfo)
            : base($"The {definition} \"{navigable.GetType().FullName}\" was not found. {supplementalInfo}")
        {
        }

        public NavigableNotFoundException(INavigable navigable)
            : base($"Could not find the Navigable \"{navigable.GetType().FullName}\".")
        {
        }
    }
}