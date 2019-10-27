using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.Extensions.Appium;
using IC.TimeoutEx;
using Moq;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Threading;
using Xunit;

namespace IC.Navigation.UnitTests
{
    public class WebElementExTests : IDisposable
    {
        public WebElementExTests()
        {
            sut = new Fixture().Customize(new AutoMoqCustomization()).Create<IWebElement>();
            cts = new CancellationTokenSource(500.Milliseconds());
            ct = cts.Token;
        }

        private readonly IWebElement sut;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken ct;

        [Theory]
        [InlineData("IsEnabled", "true")]
        [InlineData("IsEnabled", "false")]
        public void WaitUntil_With_Tuple_And_Timeout_Should_Returns_Expected_Value_Before_Timeout(
            string attributeName,
            string attributeValue)
        {
            // Arrange
            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 50.ms();

            // Act
            var actual = sut.WaitUntil(timeout, (attributeName, attributeValue));
            stopwatch.Stop();

            // Assert
            Assert.True(actual);
            Assert.True(stopwatch.ElapsedMilliseconds < timeout.Ticks);
        }

        [Theory]
        [InlineData("IsEnabled", "true")]
        [InlineData("IsEnabled", "false")]
        public void WaitUntil_With_Tuple_And_CancellationToken_Should_Returns_Expected_Value_Before_Cancellation(
            string attributeName,
            string attributeValue)
        {
            // Arrange

            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);
            var stopwatch = Stopwatch.StartNew();

            // Act
            var actual = sut.WaitUntil(ct, (attributeName, attributeValue));
            stopwatch.Stop();

            // Assert
            Assert.True(actual);
            Assert.False(ct.IsCancellationRequested);
        }

        [Theory]
        [InlineData("IsEnabled", "true")]
        [InlineData("IsEnabled", "false")]
        public void WaitUntil_With_One_Attribute_And_CancellationToken_Should_Returns_Expected_Value_Before_Cancellation(
            string attributeName,
            string attributeValue)
        {
            // Arrange

            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);

            // Act
            var actual = sut.WaitUntil(ct, attributeName, attributeValue);

            // Assert
            Assert.True(actual);
            Assert.False(ct.IsCancellationRequested);
        }

        [Theory]
        [InlineData("IsEnabled", "true")]
        [InlineData("IsEnabled", "false")]
        public void WaitUntil_With_One_Attribute_And_Timeout_Should_Returns_Expected_Value_Before_Timeout(
            string attributeName,
            string attributeValue)
        {
            // Arrange

            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 50.ms();

            // Act
            var actual = sut.WaitUntil(timeout, attributeName, attributeValue);
            stopwatch.Stop();

            // Assert
            Assert.True(actual);
            Assert.True(stopwatch.ElapsedMilliseconds < timeout.Ticks);
        }

        public void Dispose()
        {
            cts?.Dispose();
        }
    }
}