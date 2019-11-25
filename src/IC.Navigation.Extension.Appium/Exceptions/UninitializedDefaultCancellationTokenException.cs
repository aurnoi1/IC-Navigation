using System;

namespace IC.Navigation.Extensions.Exceptions
{
    public class UninitializedDefaultCancellationTokenException : Exception
    {
        public UninitializedDefaultCancellationTokenException()
            : base($"The DefaultCancellationToken is uninitialized..")
        {
        }
    }
}