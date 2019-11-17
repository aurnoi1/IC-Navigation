﻿using AutoFixture.Xunit2;
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

namespace IC.Navigation.Extensions.UnitTests.SearchProperties.Find
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
                string locatorValue
                )
            {
                // Arrange
                using var cancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                var cancellationToken = cancellationTokenSource.Token;
                var expected = webElements.ElementAt(index);
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, locatorValue)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(locator, locatorValue, webDriver, index);

                // Act
                var actual = sut.Find(cancellationToken);

                // Assert
                actual.ShouldBe(expected);
            }

            [Theory, AutoMoqData]
            public void When_index_is_out_of_range_Then_throws_OperationCanceledException(
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string locatorValue)
            {
                // Arrange
                using var cancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                int indexOutOfRange = webElements.Count + 1;
                var cancellationToken = cancellationTokenSource.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, locatorValue)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(locator, locatorValue, webDriver, indexOutOfRange);

                // Assert
                Assert.Throws<OperationCanceledException>(() => sut.Find(cancellationToken));
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
                string locatorValue
                )
            {
                // Arrange
                using var defaultCancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                var defaultCancellationToken = defaultCancellationTokenSource.Token;
                using var cancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                var cancellationToken = cancellationTokenSource.Token;
                var expected = webElements.ElementAt(index);
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, locatorValue)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(locator, locatorValue, webDriver, index, defaultCancellationToken);

                // Act
                var actual = sut.Find(cancellationToken);

                // Assert
                actual.ShouldBe(expected);
            }

            [Theory, AutoMoqData]
            public void When_index_is_out_of_range_Then_throws_OperationCanceledException(
                IFindsByFluentSelector<IWebElement> webDriver,
                [Frozen]IReadOnlyCollection<IWebElement> webElements,
                string locator,
                string locatorValue)
            {
                // Arrange
                using var defaultCancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                var defaultCancellationToken = defaultCancellationTokenSource.Token;
                using var cancellationTokenSource = new CancellationTokenSource(50.Milliseconds());
                int indexOutOfRange = webElements.Count + 1;
                var cancellationToken = cancellationTokenSource.Token;
                Mock.Get(webDriver).Setup(x => x.FindElements(locator, locatorValue)).Returns(webElements);
                var sut = new SearchProperties<IWebElement>(locator, locatorValue, webDriver, indexOutOfRange, defaultCancellationToken);

                // Assert
                Assert.Throws<OperationCanceledException>(() => sut.Find(cancellationToken));
            }
        }
    }
}