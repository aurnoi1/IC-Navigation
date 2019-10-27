using System;

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