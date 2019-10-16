using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Exceptions
{
    public class UninitializedGlobalCancellationTokenException : Exception
    {
        public UninitializedGlobalCancellationTokenException() 
            : base($"The GlobalCancellationToken is uninitialized.")
        {
        }
    }
}
