using System;
using System.Diagnostics.CodeAnalysis;

namespace IC.TimeoutEx
{
    public static class DoubleEx
    {
        /// <summary>
        /// Return a TimeSpan from seconds.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        [SuppressMessage("Style", "IDE1006:Naming Styles",
            Justification = "To respect International System of Units.")]
        public static TimeSpan s(this double timeout)
        {
            return TimeSpan.FromSeconds(timeout);
        }

        /// <summary>
        /// Return a TimeSpan from seconds.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        public static TimeSpan Seconds(this double timeout)
        {
            return timeout.s();
        }

        /// <summary>
        /// Return a TimeSpan from milliseconds.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        [SuppressMessage("Style", "IDE1006:Naming Styles",
            Justification = "To respect International System of Units.")]
        public static TimeSpan ms(this double timeout)
        {
            return TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Return a TimeSpan from milliseconds.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        public static TimeSpan Milliseconds(this double timeout)
        {
            return timeout.ms();
        }

        /// <summary>
        /// Return a TimeSpan from minutes.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        [SuppressMessage("Style", "IDE1006:Naming Styles",
            Justification = "To respect International System of Units.")]
        public static TimeSpan m(this double timeout)
        {
            return TimeSpan.FromMinutes(timeout);
        }

        /// <summary>
        /// Return a TimeSpan from minutes.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        public static TimeSpan Minutes(this double timeout)
        {
            return timeout.m();
        }

        /// <summary>
        /// Return a TimeSpan from hours.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        [SuppressMessage("Style", "IDE1006:Naming Styles",
            Justification = "To respect International System of Units.")]
        public static TimeSpan h(this double timeout)
        {
            return TimeSpan.FromHours(timeout);
        }

        /// <summary>
        /// Return a TimeSpan from hours.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The Timeout.</returns>
        public static TimeSpan Hours(this double timeout)
        {
            return timeout.h();
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