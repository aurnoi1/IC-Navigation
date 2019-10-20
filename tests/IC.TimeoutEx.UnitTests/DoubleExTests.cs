using System;
using Xunit;

namespace IC.TimeoutEx.UnitTests
{
    public class DoubleExTests
    {
        [Theory]
        [InlineData(1.1)]
        [InlineData(0)]
        [InlineData(-1.1)]
        public void s_Should_Returns_Seconds(double value)
        {
            var expected = TimeSpan.FromSeconds(value);
            var actual = value.s();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Throws_OverflowException_When_TimeSpan_MinValue()
        {
            Action actual = () => Convert.ToDouble(TimeSpan.MinValue.Ticks).s();
            Assert.Throws<OverflowException>(() => actual());
        }

        [Fact]
        public void Should_Throws_OverflowException_When_TimeSpan_MaxValue()
        {
            Action actual = () => Convert.ToDouble(TimeSpan.MaxValue.Ticks).s();
            Assert.Throws<OverflowException>(() => actual());
        }

        [Fact]
        public void Seconds_Should_Returns_Seconds()
        {
            double sut = 5.5;
            var expected = TimeSpan.FromSeconds(sut);

            var actual = sut.Seconds();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Negative_s_Should_Returns_Negative_Seconds()
        {
            double sut = -5.5;
            var expected = TimeSpan.FromSeconds(sut);

            var actual = sut.s();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ms_Should_Returns_Milliseconds()
        {
            double sut = 5.5;
            var expected = TimeSpan.FromMilliseconds(sut);

            var actual = sut.ms();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Milliseconds_Should_Returns_Milliseconds()
        {
            double sut = 5.5;
            var expected = TimeSpan.FromMilliseconds(sut);

            var actual = sut.Milliseconds();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Negative_ms_Should_Returns_Negative_Milliseconds()
        {
            double sut = -5.5;
            var expected = TimeSpan.FromMilliseconds(sut);

            var actual = sut.ms();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void m_Should_Returns_Minutes()
        {
            double sut = 5.5;
            var expected = TimeSpan.FromMinutes(sut);

            var actual = sut.m();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Minutes_Should_Returns_Minutes()
        {
            double sut = 5.5;
            var expected = TimeSpan.FromMinutes(sut);

            var actual = sut.Minutes();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Negative_m_Should_Returns_Negative_Minutes()
        {
            double sut = -5.5;
            var expected = TimeSpan.FromMinutes(sut);

            var actual = sut.m();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void h_Should_Returns_Hours()
        {
            double sut = 5.5;
            var expected = TimeSpan.FromHours(sut);

            var actual = sut.h();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Hours_Should_Returns_Hours()
        {
            double sut = 5.5;
            var expected = TimeSpan.FromHours(sut);

            var actual = sut.Hours();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Negative_h_Should_Returns_Negative_Hours()
        {
            double sut = -5.5;
            var expected = TimeSpan.FromHours(sut);

            var actual = sut.h();
            Assert.Equal(expected, actual);
        }
    }
}