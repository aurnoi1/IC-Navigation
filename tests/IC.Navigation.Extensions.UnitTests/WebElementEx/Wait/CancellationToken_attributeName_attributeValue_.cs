using IC.Navigation.Extensions.Appium;
using IC.Navigation.UnitTests.DataAttributes;
using Moq;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Threading;
using Xunit;

namespace IC.Navigation.Extensions.UnitTests.WebElementEx.Wait
{
    public class CancellationToken_attributeName_attributeValue_
    {
        [Theory, WebElementExValidData]
        public void When_attribute_match_expected_value_Then_returns_T_webElement(
            IWebElement sut,
            CancellationToken cancellationToken,
            string attributeName,
            string attributeValue)
        {
            // Arrange
            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);

            // Act
            var actual = sut.Wait(cancellationToken, attributeName, attributeValue);

            // Assert
            actual.ShouldBe(sut);
            cancellationToken.IsCancellationRequested.ShouldBeFalse();
        }

        [Theory, WebElementExCancelledTokenData]
        public void When_cancellationToken_is_canceled_Then_throws_OperationCanceledException(
            IWebElement sut,
            CancellationToken cancellationToken,
            string attributeName,
            string attributeValue)
        {
            // Arrange
            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);

            // Act
            Assert.Throws<OperationCanceledException>(() => sut.Wait(cancellationToken, attributeName, attributeValue));

            // Assert
            sut.ShouldNotBeNull();
            cancellationToken.IsCancellationRequested.ShouldBeTrue();
        }

        [Theory, WebElementExValidData]
        public void When_attribute_do_not_match_expected_value_Then_throws_OperationCanceledException(
            IWebElement sut,
            CancellationToken cancellationToken,
            string attributeName,
            string attributeValue)
        {
            // Arrange
            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);

            // Act
            Assert.Throws<OperationCanceledException>(() => sut.Wait(cancellationToken, ("invalidAttribName", "True")));

            // Assert
            sut.ShouldNotBeNull();
            cancellationToken.IsCancellationRequested.ShouldBeTrue();
        }
    }
}