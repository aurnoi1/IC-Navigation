using AutoFixture.Xunit2;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Exceptions;
using IC.Navigation.UnitTests.DataAttributes;
using IC.TimeoutEx;
using Moq;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace IC.Navigation.Extensions.UnitTests.SearchProperties.Find
{
    public class Default_
    {
        public class Given_no_defaultCancellationToken_is_initialized_
        {
            [Theory]
            [InlineAutoMoqData(0)]
            public void When_try_to_find_Then_throws_UninitializedDefaultCancellationTokenException(
                int index,
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string value
                )
            {
                // Arrange
                var expected = webElements.ElementAt(index);
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(
                    locator,
                    value,
                    webDriver,
                    index);

                // Act, Assert
                Assert.Throws<UninitializedDefaultCancellationTokenException>(() => sut.Find());
            }
        }

        public class Given_an_expired_defaultCancellationToken_3_webElements_with_same_locator_properties_
        {
            [Theory]
            [InlineAutoMoqData(0)]
            public void When_try_to_find_Then_throws_OperationCanceledException(
                int index,
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string value
                )
            {
                // Arrange
                var expected = webElements.ElementAt(index);
                using var defaultCancellationTokenSource = new CancellationTokenSource(TimeSpan.Zero);
                var expiredDefaultCancellationToken = defaultCancellationTokenSource.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(
                    locator,
                    value,
                    webDriver,
                    index,
                    expiredDefaultCancellationToken);

                // Act, Assert
                Assert.Throws<OperationCanceledException>(() => sut.Find());
            }

            [Theory, AutoMoqData]
            public void When_index_is_out_of_range_Then_throws_OperationCanceledException(
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string value)
            {
                // Arrange
                using var defaultCancellationTokenSource = new CancellationTokenSource(50.ms());
                var defaultCancellationToken = defaultCancellationTokenSource.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                int indexOutOfRange = webElements.Count + 1;
                var sut = new SearchProperties<IWebElement>(
                    locator,
                    value,
                    webDriver,
                    indexOutOfRange,
                    defaultCancellationToken);

                // Act, Assert
                Assert.Throws<OperationCanceledException>(() => sut.Find());
            }
        }

        public class Given_a_defaultCancellationToken_And_3_webElements_with_same_locator_properties_
        {
            [Theory]
            [InlineAutoMoqData(0)]
            [InlineAutoMoqData(1)]
            [InlineAutoMoqData(2)]
            public void When_index_is_between_0_and_2_Then_returns_webElement_at_matching_index(
                int index,
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string value
                )
            {
                // Arrange
                using var defaultCancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                var defaultCancellationToken = defaultCancellationTokenSource.Token;
                var expected = webElements.ElementAt(index);
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(
                    locator,
                    value,
                    webDriver,
                    index,
                    defaultCancellationToken);

                // Act
                var actual = sut.Find();

                // Assert
                actual.ShouldBe(expected);
            }

            [Theory, AutoMoqData]
            public void When_index_is_out_of_range_Then_throws_OperationCanceledException(
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string value)
            {
                // Arrange
                using var defaultCancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                var defaultCancellationToken = defaultCancellationTokenSource.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                int indexOutOfRange = webElements.Count + 1;
                var sut = new SearchProperties<IWebElement>(
                    locator,
                    value,
                    webDriver,
                    indexOutOfRange,
                    defaultCancellationToken);

                // Act, Assert
                Assert.Throws<OperationCanceledException>(() => sut.Find());
            }


            [Theory, AutoMoqData]
            public void When_index_is_not_defined_Then_returns_first_WebElement(
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string value)
            {
                // Arrange
                using var defaultCancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                var defaultCancellationToken = defaultCancellationTokenSource.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(locator, value, webDriver, defaultCancellationToken);

                // Act
                var actual = sut.Find();

                // Assert
                actual.ShouldBe(webElements.First());
            }
        }
    }
}