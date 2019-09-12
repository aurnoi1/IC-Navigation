using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Exceptions
{
    public class EntryPointsNotFoundException : Exception
    {
        public EntryPointsNotFoundException()
        {
        }

        public EntryPointsNotFoundException(IEnumerable<INavigable> entryPointsTypes) 
            : base($"Could not find any EntryPoint: {string.Join(",", entryPointsTypes.GetType().FullName)}.")
        {
        }
    }
}
