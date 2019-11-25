using System;

namespace IC.Navigation.Extension.Appium.Exceptions
{
    public class UninitializedDefaultCancellationTokenException : Exception
    {
        public UninitializedDefaultCancellationTokenException()
            : base($"The DefaultCancellationToken is uninitialized..")
        {
        }
    }
}