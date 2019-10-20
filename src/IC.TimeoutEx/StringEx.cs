using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
