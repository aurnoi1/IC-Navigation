using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IC.TimeoutEx
{
    public class TimeoutEx
    {
        /// <summary>
        /// The Dictionary of patterns and associated transformation function.
        /// </summary>
        public static Dictionary<string, Func<double, TimeSpan>> Patterns => _Patterns;

        /// <summary>
        /// The regex used to find a value inside a pattern.
        /// </summary>
        public static string ValuePattern = @"(\d+|-\d+)"; // Must be declared before patterns.

        private static Dictionary<string, Func<double, TimeSpan>> _Patterns { get; set; } = 
            new Dictionary<string, Func<double, TimeSpan>>()
            {
                { $@"^{ValuePattern} seconds$", x => x.s() },
                { $@"^{ValuePattern} milliseconds$", x => x.ms() },
                { $@"^{ValuePattern} minutes$", x => x.m() },
                { $@"^{ValuePattern} hours$", x => x.h() },
            };

        #region Methods

        /// <summary>
        /// Add a custom pattern and its associated transformation function to the Patterns dictionary.
        /// </summary>
        /// <param name="customPattern">The custom pattern to add.</param>
        /// <param name="func">The transformation function to associate with the custom pattern.</param>
        /// <exception cref="Exception">Thrown when a matching pattern already exists in Patterns dictionary.</exception>
        /// <exception cref="ArgumentException">Thrown when the pattern does not contains the <see cref="ValuePattern"/>.</exception>
        public static void AddPatterns(string customPattern, Func<double, TimeSpan> func)
        {
            var keyValue = Patterns.Where(x => Regex.IsMatch(customPattern, x.Key)).SingleOrDefault();
            if (keyValue.Key != default)
            {
                throw new Exception($"Cannot register \"{customPattern}\" because \"{keyValue.Key}\" " +
                    $"already match this pattern.");
            }

            if (!customPattern.Contains(ValuePattern))
            {
                throw new ArgumentException($"Cannot get value from \"{customPattern}\". " +
                    $"Ensure the pattern contains the ValuePattern \"{ValuePattern}\".");
            }

            Patterns.Add(customPattern, func);
        }

        /// <summary>
        /// Remove a pattern from the Patterns dictionary.
        /// </summary>
        /// <param name="pattern">The pattern to remove.</param>
        public static void RemovePattern(string pattern)
        {
            _Patterns.Remove(pattern);
        }

        /// <summary>
        /// Gets value from pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>The value.</returns>
        public static double GetValue(string pattern)
        {
            var match = GetMatch(pattern);
            if (!double.TryParse(match.Value, out double value))
            {
                throw new ArgumentException($"Cannot convert\"{match.Value}\" to double.");
            }

            return value;
        }

        /// <summary>
        /// Transform a timeout as string to TimeSpan.
        /// </summary>
        /// <param name="timeout">The timeout as string.</param>
        /// <returns>The timeout as TimeSpan.</returns>
        public static TimeSpan TransformToTimeSpan(string timeout)
        {
            double value = GetValue(timeout);
            var func = GetFunc(timeout);
            return func(value);
        }

        private static Match GetMatch(string pattern)
        {
            var match = Regex.Match(pattern, ValuePattern);
            return match;
        }

        private static Func<double, TimeSpan> GetFunc(string timeout)
        {
            var keyValue = Patterns.Where(x => Regex.IsMatch(timeout, x.Key)).SingleOrDefault();
            if (keyValue.Value == default)
            {
                throw new Exception($"There is no pattern matching the argument \"{timeout}\".");
            }

            return keyValue.Value;
        }

        #endregion Methods
    }
}