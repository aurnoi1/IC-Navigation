using IC.Navigation.Extension.Appium;
using IC.Navigation.Extension.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class SearchPropertiesCancellationTests : IDisposable
    {
        public SearchPropertiesCancellationTests()
        {
            sut = new WindowsContext<WindowsDriver<WindowsElement>>().AppBrowser;
        }

        #region Properties

        #region Private

        private readonly IAppBrowser<WindowsDriver<WindowsElement>> sut;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void Get_With_DefaultCancellationToken_Should_Returns_Control_Matching()
        {
            // Arrange
            using var longCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                longCts.Token);

            // Act
            var titleMenu = titleMenuSearchProp.Get();

            // Assert
            Assert.NotNull(titleMenu);
        }

        [Fact]
        public void Get_With_DefaultCancellationToken_Should_Returns_Null_On_DefaultCancellationToken_Cancelled()
        {
            // Arrange
            using var expiredCts = new CancellationTokenSource(TimeSpan.Zero);
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                expiredCts.Token);

            // Act
            var titleMenu = titleMenuSearchProp.Get();

            // Assert
            Assert.Null(titleMenu);
            Assert.True(expiredCts.IsCancellationRequested);
        }

        [Fact]
        public void Get_With_Timeout_Should_Returns_Null_On_DefaultCancellationToken_Cancelled()
        {
            // Arrange
            using var expiredCts = new CancellationTokenSource(TimeSpan.Zero);
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                expiredCts.Token);

            // Act
            var titleMenu = titleMenuSearchProp.Get(TimeSpan.FromSeconds(5));

            // Assert
            Assert.Null(titleMenu);
            Assert.True(expiredCts.IsCancellationRequested);
        }

        [Fact]
        public void Get_With_Many_CancellationTokens_Should_Returns_Null_On_DefaultCancellationToken_Cancelled()
        {
            // Arrange
            using var expiredCts = new CancellationTokenSource(TimeSpan.Zero);
            using var longCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var longCts2 = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                expiredCts.Token);

            // Act
            var titleMenu = titleMenuSearchProp.Get(longCts.Token, longCts2.Token);

            // Assert
            Assert.Null(titleMenu);
            Assert.True(expiredCts.IsCancellationRequested);
            Assert.False(longCts.IsCancellationRequested);
            Assert.False(longCts2.IsCancellationRequested);
        }

        [Fact]
        public void Get_With_Many_CancellationTokens_Should_Returns_Null_On_Any_Cancelled()
        {
            // Arrange
            using var expiredCts = new CancellationTokenSource(TimeSpan.Zero);
            using var longCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var longCts2 = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                longCts.Token);

            // Act
            var titleMenu = titleMenuSearchProp.Get(expiredCts.Token, longCts2.Token);

            // Assert
            Assert.Null(titleMenu);
            Assert.True(expiredCts.IsCancellationRequested);
            Assert.False(longCts.IsCancellationRequested);
            Assert.False(longCts2.IsCancellationRequested);
        }

        [Fact]
        public void Find_With_DefaultCancellationToken_Should_Throws_OperationCanceledException()
        {
            // Arrange
            using var expiredCts = new CancellationTokenSource(TimeSpan.Zero);
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                expiredCts.Token);

            // Act
            Func<WindowsElement> find = () => titleMenuSearchProp.Find();

            // Assert
            Assert.Throws<OperationCanceledException>(() => find());
        }

        [Fact]
        public void Find_With_Timeout_And_DefaultCancellationToken_Should_Throws_OperationCanceledException()
        {
            // Arrange
            using var expiredCts = new CancellationTokenSource(TimeSpan.Zero);
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                expiredCts.Token);

            // Act
            Func<WindowsElement> find = () => titleMenuSearchProp.Find(TimeSpan.FromSeconds(10));

            // Assert
            var exception = Assert.Throws<TimeoutException>(() => find());
            Assert.Equal("The timeout has been reached before the Element could be found.", exception.Message);
        }

        [Fact]
        public void Find_With_Timeout_And_DefaultCancellationToken_Should_Throws_TimeoutException()
        {
            // Arrange
            using var expiredCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                expiredCts.Token);

            // Act
            Func<WindowsElement> find = () => titleMenuSearchProp.Find(TimeSpan.Zero);

            // Assert
            Assert.Throws<TimeoutException>(() => find());
        }

        [Fact]
        public void Find_With_Token_Should_Throws_OperationCanceledException_On_DefaultCancellationToken_Cancelled()
        {
            // Arrange
            using var longCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var expiredCts = new CancellationTokenSource(TimeSpan.Zero);
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                expiredCts.Token);

            // Act
            Func<WindowsElement> find = () => titleMenuSearchProp.Find(longCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => find());
            Assert.True(expiredCts.IsCancellationRequested);
            Assert.False(longCts.IsCancellationRequested);
        }

        [Fact]
        public void Find_With_Token_Should_Throws_OperationCanceledException_On_LocalToken_Cancelled()
        {
            // Arrange
            using var longCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var expiredCts = new CancellationTokenSource(TimeSpan.Zero);
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                longCts.Token);

            // Act
            Func<WindowsElement> find = () => titleMenuSearchProp.Find(expiredCts.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => find());
            Assert.True(expiredCts.IsCancellationRequested);
            Assert.False(longCts.IsCancellationRequested);
        }

        [Fact]
        public void Find_With_Many_CancellationTokens_Should_Throws_OperationCanceledException_On_Any_Cancelled()
        {
            // Arrange
            using var longCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var longCts2 = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            using var expiredCts = new CancellationTokenSource(TimeSpan.Zero);
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                longCts.Token);

            // Act
            Func<WindowsElement> find = () => titleMenuSearchProp.Find(expiredCts.Token, longCts2.Token);

            // Assert
            Assert.Throws<OperationCanceledException>(() => find());
            Assert.True(expiredCts.IsCancellationRequested);
            Assert.False(longCts.IsCancellationRequested);
            Assert.False(longCts2.IsCancellationRequested);
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}