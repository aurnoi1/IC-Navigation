using AutoFixture.Xunit2;
using IC.Navigation.Extension.Appium;
using IC.Navigation.Extension.UnitTests.DataAttributes;
using Moq;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Threading;
using Xunit;

namespace IC.Navigation.Extension.UnitTests.WebElementEx.Wait
{
    public class CancellationToken_attributeName_attributeValue_
    {
        [Theory, AutoMoqData]
        public void When_attribute_match_expected_value_Then_returns_T_webElement(
            [Frozen]IWebElement sut,
            string attributeName,
            string attributeValue)
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
            var cancellationToken = cancellationTokenSource.Token;
            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);

            // Act
            var actual = sut.Wait(cancellationToken, attributeName, attributeValue);

            // Assert
            actual.ShouldBe(sut);
            cancellationToken.IsCancellationRequested.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        public void When_cancellationToken_is_canceled_Then_throws_OperationCanceledException(
            [Frozen]IWebElement sut,
            string attributeName,
            string attributeValue)
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.Zero);
            var expiredCancellationToken = cancellationTokenSource.Token;
            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);

            // Act
            Assert.Throws<OperationCanceledException>(() => sut.Wait(expiredCancellationToken, attributeName, attributeValue));

            // Assert
            sut.ShouldNotBeNull();
            expiredCancellationToken.IsCancellationRequested.ShouldBeTrue();
        }

        [Theory, AutoMoqData]
        public void When_attribute_do_not_match_expected_value_Then_throws_OperationCanceledException(
            [Frozen]IWebElement sut,
            string attributeName,
            string attributeValue)
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
            var cancellationToken = cancellationTokenSource.Token;
            Mock.Get(sut).Setup(x => x.GetAttribute(attributeName)).Returns(attributeValue);

            // Act
            Assert.Throws<OperationCanceledException>(() => sut.Wait(cancellationToken, ("invalidAttribName", "True")));

            // Assert
            sut.ShouldNotBeNull();
            cancellationToken.IsCancellationRequested.ShouldBeTrue();
        }
    }
}