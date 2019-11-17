using AutoFixture.Xunit2;
using IC.Navigation.Extensions.Appium;
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

namespace IC.Navigation.Extensions.UnitTests.SearchProperties.Get
{
    public class CancellationTokens_
    {
        public class Given_3_webElements_with_same_locator_properties_
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
                var expected = webElements.ElementAt(index);
                using var cancellationTokenSource1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
                using var cancellationTokenSource2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(70));
                var cancellationToken1 = cancellationTokenSource1.Token;
                var cancellationToken2 = cancellationTokenSource2.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(locator, value, webDriver, index);

                // Act
                var actual = sut.Get(cancellationToken1, cancellationToken2);

                // Assert
                actual.ShouldBe(expected);
            }

            [Theory, AutoMoqData]
            public void When_index_is_out_of_range_Then_returns_null(
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string value)
            {
                // Arrange
                using var cancellationTokenSource1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
                using var cancellationTokenSource2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(70));
                var cancellationToken1 = cancellationTokenSource1.Token;
                var cancellationToken2 = cancellationTokenSource2.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                int indexOutOfRange = webElements.Count + 1;
                var sut = new SearchProperties<IWebElement>(locator, value, webDriver, indexOutOfRange);

                // Act
                var actual = sut.Get(cancellationToken1, cancellationToken2);

                // Assert
                actual.ShouldBeNull();
            }

            [Theory, AutoMoqData]
            public void When_index_is_not_defined_Then_returns_first_WebElement(
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string value)
            {
                // Arrange
                using var cancellationTokenSource1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
                using var cancellationTokenSource2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(70));
                var cancellationToken1 = cancellationTokenSource1.Token;
                var cancellationToken2 = cancellationTokenSource2.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(locator, value, webDriver);

                // Act
                var actual = sut.Get(cancellationToken1, cancellationToken2);

                // Assert
                actual.ShouldBe(webElements.First());
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
                using var cancellationTokenSource1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
                using var cancellationTokenSource2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(70));
                var cancellationToken1 = cancellationTokenSource1.Token;
                var cancellationToken2 = cancellationTokenSource2.Token;
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
                var actual = sut.Get(cancellationToken1, cancellationToken2);

                // Assert
                actual.ShouldBe(expected);
            }

            [Theory, AutoMoqData]
            public void When_index_is_out_of_range_Then_returns_null(
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string value)
            {
                // Arrange
                using var defaultCancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                using var cancellationTokenSource1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
                using var cancellationTokenSource2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(70));
                var cancellationToken1 = cancellationTokenSource1.Token;
                var cancellationToken2 = cancellationTokenSource2.Token;
                var defaultCancellationToken = defaultCancellationTokenSource.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                int indexOutOfRange = webElements.Count + 1;
                var sut = new SearchProperties<IWebElement>(locator, value, webDriver, indexOutOfRange, defaultCancellationToken);

                // Act
                var actual = sut.Get(cancellationToken1, cancellationToken2);

                // Assert
                actual.ShouldBeNull();
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
                using var cancellationTokenSource1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
                using var cancellationTokenSource2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(70));
                var cancellationToken1 = cancellationTokenSource1.Token;
                var cancellationToken2 = cancellationTokenSource2.Token;
                var defaultCancellationToken = defaultCancellationTokenSource.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, value)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(locator, value, webDriver, defaultCancellationToken);

                // Act
                var actual = sut.Get(cancellationToken1, cancellationToken2);

                // Assert
                actual.ShouldBe(webElements.First());
            }
        }
    }
}