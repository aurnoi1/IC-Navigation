using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IC.TimeoutEx
{
    public class TimeoutEx
    {
        /// <summary>
        /// The Dictionary of patterns and associated transformation functions.
        /// </summary>
        public static Dictionary<string, Func<double[], TimeSpan>> Patterns => _Patterns;

        /// <summary>
        /// The regex used to find a value inside a pattern.
        /// </summary>
        public const string ValuePattern = @"(-?\d+(?:\.\d+)?)"; // Must be declared before patterns.

        private static Dictionary<string, Func<double[], TimeSpan>> _Patterns { get; set; } =
            new Dictionary<string, Func<double[], TimeSpan>>()
            {
                { $@"^{ValuePattern} seconds$", x => x.Single().s() },
                { $@"^{ValuePattern} milliseconds$", x => x.Single().ms() },
                { $@"^{ValuePattern} minutes$", x => x.Single().m() },
                { $@"^{ValuePattern} hours$", x => x.Single().h() },
            };

        #region Methods

        /// <summary>
        /// Add a custom pattern and its associated transformation function to the Patterns dictionary.
        /// </summary>
        /// <param name="customPattern">The custom pattern to add.</param>
        /// <param name="func">The transformation function to associate with the custom pattern.</param>
        /// <exception cref="Exception">Thrown when a matching pattern already exists in Patterns dictionary.</exception>
        /// <exception cref="ArgumentException">Thrown when the pattern does not contains the <see cref="ValuePattern"/>.</exception>
        public static void AddPatterns(string customPattern, Func<double[], TimeSpan> func)
        {
            var keyValue = Patterns.Where(x => Regex.IsMatch(customPattern, x.Key)).SingleOrDefault();
            if (keyValue.Key != default)
            {
                throw new Exception($"Cannot register \"{customPattern}\" because \"{keyValue.Key}\" " +
                    $"already match this pattern.");
            }

            if (!customPattern.Contains(ValuePattern))
            {
                throw new ArgumentException($"Cannot get any value from \"{customPattern}\". " +
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
        /// Gets values from pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>The values.</returns>
        public static double[] GetValues(string pattern)
        {
            var matches = GetMatches(pattern);
            var values = new List<double>();
            foreach (var match in matches)
            {
                if (!double.TryParse((match as Match).Value, out double value))
                {
                    throw new ArgumentException($"Cannot convert\"{match}\" to double.");
                }

                values.Add(value);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Transform a timeout as string to TimeSpan.
        /// </summary>
        /// <param name="timeout">The timeout as string.</param>
        /// <returns>The timeout as TimeSpan.</returns>
        public static TimeSpan TransformToTimeSpan(string timeout)
        {
            double[] value = GetValues(timeout);
            var func = GetFunc(timeout);
            return func(value);
        }

        private static MatchCollection GetMatches(string pattern)
        {
            var match = Regex.Matches(pattern, ValuePattern);
            return match;
        }

        private static Func<double[], TimeSpan> GetFunc(string timeout)
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