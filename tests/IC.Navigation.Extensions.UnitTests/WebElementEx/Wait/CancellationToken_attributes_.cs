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
        [Theory, WebElementExValidData]
        public void When_attributes_match_expected_values_Then_returns_T_webElement(
            IWebElement sut,
            CancellationToken cancellationToken,
            Dictionary<string, string> attributes)
        {
            // Arrange
            attributes.ToList().ForEach(p => Mock.Get(sut).Setup(x => x.GetAttribute(p.Key)).Returns(p.Value));

            // Act
            var actual = sut.Wait(cancellationToken, attributes);

            // Assert
            actual.ShouldBe(sut);
            cancellationToken.IsCancellationRequested.ShouldBeFalse();
        }

        [Theory, WebElementExCancelledTokenData]
        public void When_cancellationToken_is_canceled_Then_throws_OperationCanceledException(
            IWebElement sut,
            CancellationToken cancellationToken,
            Dictionary<string, string> attributes)
        {
            // Arrange
            attributes.ToList().ForEach(p => Mock.Get(sut).Setup(x => x.GetAttribute(p.Key)).Returns(p.Value));

            // Act
            Assert.Throws<OperationCanceledException>(() => sut.Wait(cancellationToken, attributes));

            // Assert
            sut.ShouldNotBeNull();
            cancellationToken.IsCancellationRequested.ShouldBeTrue();
        }
    }
}