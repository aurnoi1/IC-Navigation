using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace IC.TimeoutEx.UnitTests
{
    public class StringExTests
    {
        [Theory]
        [InlineData(10, "10 seconds")]
        [InlineData(0, "0 seconds")]
        [InlineData(-10, "-10 seconds")]
        public void ToTimeSpan_Should_Convert_To_TimeSpan_In_Seconds(double value, string timeout)
        {
            var expected = TimeSpan.FromSeconds(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "10 minutes")]
        [InlineData(0, "0 minutes")]
        [InlineData(-10, "-10 minutes")]
        public void ToTimeSpan_Should_Convert_To_TimeSpan_In_Minutes(double value, string timeout)
        {
            var expected = TimeSpan.FromMinutes(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "10 milliseconds")]
        [InlineData(0, "0 milliseconds")]
        [InlineData(-10, "-10 milliseconds")]
        public void ToTimeSpan_Should_Convert_To_TimeSpan_In_Milliseconds(double value, string timeout)
        {
            var expected = TimeSpan.FromMilliseconds(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "10 hours")]
        [InlineData(0, "0 hours")]
        [InlineData(-10, "-10 hours")]
        public void ToTimeSpan_Should_Convert_To_TimeSpan_In_Hours(double value, string timeout)
        {
            var expected = TimeSpan.FromHours(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "of 10 seconds", @"^of (\d+|-\d+) seconds$")]
        [InlineData(0, "to 0 seconds", @"^to (\d+|-\d+) seconds$")]
        [InlineData(-5, "in -5 seconds", @"^in (\d+|-\d+) seconds$")]
        public void TimeoutEx_Should_Allows_Add_And_Remove_Custom_Patterns(double value, string timeout, string customPattern)
        {
            TimeoutEx.AddPatterns(customPattern, val => val.s());
            var expected = TimeSpan.FromSeconds(value);
            var actual = timeout.ToTimeSpan();
            Assert.Equal(expected, actual);
            TimeoutEx.RemovePattern(customPattern);
        }
    }
}