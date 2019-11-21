using IC.Navigation.Extensions.Appium;
using IC.Navigation.UnitTests.DataAttributes;
using Moq;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace IC.Navigation.Extensions.UnitTests.WebElementEx.Wait
{
    public class CancellationToken_attributes_
    {
        [Theory, AutoMoqData]
        public void When_attributes_match_expected_values_Then_returns_T_webElement(
            IWebElement sut,
            Dictionary<string, string> attributes)
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            var cancellationToken = cancellationTokenSource.Token;
            attributes.ToList().ForEach(p => Mock.Get(sut).Setup(x => x.GetAttribute(p.Key)).Returns(p.Value));

            // Act
            var actual = sut.Wait(cancellationToken, attributes);

            // Assert
            actual.ShouldBe(sut);
            cancellationToken.IsCancellationRequested.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        public void When_cancellationToken_is_canceled_Then_throws_OperationCanceledException(
            IWebElement sut,
            Dictionary<string, string> attributes)
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.Zero);
            var expiredCancellationToken = cancellationTokenSource.Token;
            attributes.ToList().ForEach(p => Mock.Get(sut).Setup(x => x.GetAttribute(p.Key)).Returns(p.Value));

            // Act
            Assert.Throws<OperationCanceledException>(() => sut.Wait(expiredCancellationToken, attributes));

            // Assert
            sut.ShouldNotBeNull();
            expiredCancellationToken.IsCancellationRequested.ShouldBeTrue();
        }
    }
}