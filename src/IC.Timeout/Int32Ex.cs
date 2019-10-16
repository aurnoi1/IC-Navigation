using System;

namespace IC.Timeout
{
    public static class Int32Ex
    {
        /// <summary>
        /// Return a TimeSpan from seconds.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", 
            Justification = "To respect International System of Units.")]
        public static TimeSpan s(this int timeout)
        {
            return TimeSpan.FromSeconds(timeout);
        }

        /// <summary>
        /// Return a TimeSpan from milliseconds.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", 
            Justification = "To respect International System of Units.")]
        public static TimeSpan ms(this int timeout)
        {
            return TimeSpan.FromMilliseconds(timeout);
        }


        /// <summary>
        /// Return a TimeSpan from minutes.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", 
            Justification = "To respect International System of Units.")]
        public static TimeSpan m(this int timeout)
        {
            return TimeSpan.FromMinutes(timeout);
        }

        /// <summary>
        /// Return a TimeSpan from hours.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", 
            Justification = "To respect International System of Units.")]
        public static TimeSpan h(this int timeout)
        {
            return TimeSpan.FromHours(timeout);
        }

        /// <summary>
        /// Return an infinit TimeSpan.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        public static TimeSpan Infinit()
        {
            return System.Threading.Timeout.InfiniteTimeSpan;
        }
    }
}
