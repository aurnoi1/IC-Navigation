using System;
using System.Linq;
using Xunit;

namespace IC.TimeoutEx.UnitTests
{
    public class StringExTests
    {
        [Theory]
        [InlineData(10, "10 seconds")]
        [InlineData(0, "0 seconds")]
        [InlineData(-10.2, "-10.2 seconds")]
        public void ToTimeSpan_Should_Convert_To_TimeSpan_In_Seconds(double value, string timeout)
        {
            var expected = TimeSpan.FromSeconds(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "10 minutes")]
        [InlineData(0, "0 minutes")]
        [InlineData(-10.2, "-10.2 minutes")]
        public void ToTimeSpan_Should_Convert_To_TimeSpan_In_Minutes(double value, string timeout)
        {
            var expected = TimeSpan.FromMinutes(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "10 milliseconds")]
        [InlineData(0, "0 milliseconds")]
        [InlineData(-10.2, "-10.2 milliseconds")]
        public void ToTimeSpan_Should_Convert_To_TimeSpan_In_Milliseconds(double value, string timeout)
        {
            var expected = TimeSpan.FromMilliseconds(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "10 hours")]
        [InlineData(0, "0 hours")]
        [InlineData(-10.2, "-10.2 hours")]
        public void ToTimeSpan_Should_Convert_To_TimeSpan_In_Hours(double value, string timeout)
        {
            var expected = TimeSpan.FromHours(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10.2, "of 10.2 seconds", @"^of (-?\d+(?:\.\d+)?) seconds$")]
        [InlineData(0, "to 0 seconds", @"^to (-?\d+(?:\.\d+)?) seconds$")]
        [InlineData(-5, "in -5 seconds", @"^in (-?\d+(?:\.\d+)?) seconds$")]
        public void TimeoutEx_Should_Allows_Add_And_Remove_Custom_Patterns(double value, string timeout, string customPattern)
        {
            TimeoutEx.AddPatterns(customPattern, val => val.First().s());
            var expected = TimeSpan.FromSeconds(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
            TimeoutEx.RemovePattern(customPattern);
        }

        [Theory]
        [InlineData(5, 10)]
        [InlineData(0, 10)]
        [InlineData(-5, 10)]
        [InlineData(-5, -10)]
        public void TimeoutEx_Should_Allows_Complex_Custom_Patterns_Minutes_Seconds(double minutes, double seconds)
        {
            var timeout = $"of {minutes} minutes and {seconds} seconds";
            var customPattern = $@"^of {TimeoutEx.ValuePattern} minutes and {TimeoutEx.ValuePattern} seconds$";

            static TimeSpan Minutes_And_Seconds(double[] values)
            {
                return TimeSpan.FromMinutes(values[0]) + TimeSpan.FromSeconds(values[1]);
            }

            TimeoutEx.AddPatterns(customPattern, values => Minutes_And_Seconds(values));
            var expected = TimeSpan.FromMinutes(minutes).Add(TimeSpan.FromSeconds(seconds));
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
            TimeoutEx.RemovePattern(customPattern);
        }

        [Theory]
        [InlineData(5, 10, 2000)]
        [InlineData(0, 0, 0)]
        [InlineData(-1, 0, 0)]
        public void TimeoutEx_Should_Allows_Complex_Custom_Patterns_Minutes_Seconds_Milliseconds(double minutes, double seconds, double milliseconds)
        {
            var timeout = $"of {minutes} minutes, {seconds} seconds and {milliseconds} milliseconds";
            var customPattern = $@"^of {TimeoutEx.ValuePattern} minutes, {TimeoutEx.ValuePattern} seconds and {TimeoutEx.ValuePattern} milliseconds$";

            static TimeSpan Minutes_And_Seconds(double[] values)
            {
                return TimeSpan.FromMinutes(values[0]) + TimeSpan.FromSeconds(values[1]) + TimeSpan.FromMilliseconds(values[2]);
            }

            TimeoutEx.AddPatterns(customPattern, values => Minutes_And_Seconds(values));
            var expected = TimeSpan.FromMinutes(minutes).Add(TimeSpan.FromSeconds(seconds)).Add(TimeSpan.FromMilliseconds(milliseconds));
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
            TimeoutEx.RemovePattern(customPattern);
        }
    }
}