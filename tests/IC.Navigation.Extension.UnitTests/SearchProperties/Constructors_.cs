using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using IC.Navigation.Extension.Appium;
using IC.Navigation.Extension.Appium.Interfaces;
using IC.Navigation.Extension.UnitTests.DataAttributes;
using IC.TimeoutEx;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using Shouldly;
using System.Threading;
using Xunit;

namespace IC.Navigation.Extension.UnitTests.SearchProperties
{
    public class Idioms_
    {
        [Fact]
        public void Verify()
        {
            // Assert
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(SearchProperties<IWebElement>).GetConstructors());
        }
    }

    public class Locator_value_webDriver_index_
    {
        [Theory, AutoMoqData]
        public void When_all_parameters_are_valid_Then_returns_SearchProperties(
            int index,
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange, Act
            var actual = new SearchProperties<IWebElement>(locator, value, webDriver, index);

            // Assert
            actual.ShouldNotBeNull();
            actual.ShouldBeAssignableTo<ISearchProperties<IWebElement>>();
        }

        [Theory, AutoMoqData]
        public void When_all_parameters_are_valid_Then_assigned_public_properties(
            int index,
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange, Act
            var actual = new SearchProperties<IWebElement>(locator, value, webDriver, index);

            // Assert
            actual.Locator.ShouldBe(locator);
            actual.Value.ShouldBe(value);
            actual.WebDriver.ShouldBe(webDriver);
            actual.Index.ShouldBe(index);
        }

        [Theory, AutoMoqData]
        public void When_no_defaultCancellationToken_Then_DefaultCancellationToken_is_None(
            int index,
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange, Act
            var actual = new SearchProperties<IWebElement>(locator, value, webDriver, index);

            // Assert
            actual.DefaultCancellationToken.ShouldBe(CancellationToken.None);
        }
    }

    public class Locator_value_webDriver_index_defaultCancellationToken_
    {
        [Theory, AutoMoqData]
        public void When_all_parameters_are_valid_Then_returns_SearchProperties(
            int index,
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange
            using var defaultCancellationTokenSource = new CancellationTokenSource();
            var defaultCancellationToken = defaultCancellationTokenSource.Token;

            // Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver,
                index,
                defaultCancellationToken);

            // Assert
            actual.ShouldNotBeNull();
            actual.ShouldBeAssignableTo<ISearchProperties<IWebElement>>();
        }

        [Theory, AutoMoqData]
        public void When_all_parameters_are_valid_Then_assigned_public_properties(
            int index,
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange
            using var defaultCancellationTokenSource = new CancellationTokenSource();
            var defaultCancellationToken = defaultCancellationTokenSource.Token;

            // Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver,
                index,
                defaultCancellationToken);

            // Assert
            actual.DefaultCancellationToken.ShouldBe(defaultCancellationToken);
            actual.Locator.ShouldBe(locator);
            actual.Value.ShouldBe(value);
            actual.WebDriver.ShouldBe(webDriver);
            actual.Index.ShouldBe(index);
        }

        [Theory, AutoMoqData]
        public void When_defaultCancellationToken_is_not_canceled_Then_DefaultCancellationToken_is_not_canceled(
            int index,
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange
            using var defaultCancellationTokenSource = new CancellationTokenSource(100.ms());
            var defaultCancellationToken = defaultCancellationTokenSource.Token;

            // Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver,
                index,
                defaultCancellationToken);

            // Assert
            actual.DefaultCancellationToken.IsCancellationRequested.ShouldBeFalse();
        }
    }

    public class Locator_value_webDriver_defaultCancellationToken_
    {
        [Theory, AutoMoqData]
        public void When_all_parameters_are_valid_Then_returns_SearchProperties(
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange
            using var defaultCancellationTokenSource = new CancellationTokenSource();
            var defaultCancellationToken = defaultCancellationTokenSource.Token;

            // Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver,
                defaultCancellationToken);

            // Assert
            actual.ShouldNotBeNull();
            actual.ShouldBeAssignableTo<ISearchProperties<IWebElement>>();
        }

        [Theory, AutoMoqData]
        public void When_all_parameters_are_valid_Then_assigned_public_properties(
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange
            using var defaultCancellationTokenSource = new CancellationTokenSource();
            var defaultCancellationToken = defaultCancellationTokenSource.Token;

            // Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver,
                defaultCancellationToken);

            // Assert
            actual.DefaultCancellationToken.ShouldBe(defaultCancellationToken);
            actual.Locator.ShouldBe(locator);
            actual.Value.ShouldBe(value);
            actual.WebDriver.ShouldBe(webDriver);
        }

        [Theory, AutoMoqData]
        public void When_no_index_is_defined_Then_Index_is_0(
             IFindsByFluentSelector<IWebElement> webDriver,
             string locator,
             string value)
        {
            // Arrange
            using var defaultCancellationTokenSource = new CancellationTokenSource();
            var defaultCancellationToken = defaultCancellationTokenSource.Token;

            // Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver,
                defaultCancellationToken);

            // Assert
            actual.Index.ShouldBe(0);
        }

        [Theory, AutoMoqData]
        public void When_defaultCancellationToken_is_not_canceled_Then_DefaultCancellationToken_is_not_canceled(
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange
            using var defaultCancellationTokenSource = new CancellationTokenSource(100.ms());
            var defaultCancellationToken = defaultCancellationTokenSource.Token;

            // Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver,
                defaultCancellationToken);

            // Assert
            actual.DefaultCancellationToken.IsCancellationRequested.ShouldBeFalse();
        }
    }

    public class Locator_value_webDriver_
    {
        [Theory, AutoMoqData]
        public void When_all_parameters_are_valid_Then_returns_SearchProperties(
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange, Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver);

            // Assert
            actual.ShouldNotBeNull();
            actual.ShouldBeAssignableTo<ISearchProperties<IWebElement>>();
        }

        [Theory, AutoMoqData]
        public void When_all_parameters_are_valid_Then_assigned_public_properties(
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange, Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver);

            // Assert
            actual.Locator.ShouldBe(locator);
            actual.Value.ShouldBe(value);
            actual.WebDriver.ShouldBe(webDriver);
            actual.Index.ShouldBe(0);
        }

        [Theory, AutoMoqData]
        public void When_no_defaultCancellationToken_Then_DefaultCancellationToken_is_None(
            IFindsByFluentSelector<IWebElement> webDriver,
            string locator,
            string value)
        {
            // Arrange, Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver);

            // Assert
            actual.DefaultCancellationToken.ShouldBe(CancellationToken.None);
        }

        [Theory, AutoMoqData]
        public void When_no_index_is_defined_Then_Index_is_0(
             IFindsByFluentSelector<IWebElement> webDriver,
             string locator,
             string value)
        {
            // Arrange, Act
            var actual = new SearchProperties<IWebElement>(
                locator,
                value,
                webDriver);

            // Assert
            actual.Index.ShouldBe(0);
        }
    }
}