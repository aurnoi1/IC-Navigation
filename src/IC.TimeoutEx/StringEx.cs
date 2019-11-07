using System;

namespace IC.TimeoutEx
{
    public static class StringEx
    {
        public static TimeSpan ToTimeSpan(this string timeout)
        {
            return TimeoutEx.TransformToTimeSpan(timeout);
        }
    }
}