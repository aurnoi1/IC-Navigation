using System;
using Xunit;

namespace IC.TimeoutEx.UnitTests
{
    public class EdgeValuesTests
    {
        [Fact]
        public void Should_Throws_OverflowException_When_TimeSpan_MinValue()
        {
            Action actual = () => Convert.ToDouble(TimeSpan.MinValue.Ticks).s();
            Assert.Throws<OverflowException>(() => actual());
        }

        [Fact]
        public void Should_Throws_OverflowException_When_TimeSpan_MaxValue()
        {
            Action actual = () => Convert.ToDouble(TimeSpan.MaxValue.Ticks).Seconds();
            Assert.Throws<OverflowException>(() => actual());
        }
    }
}