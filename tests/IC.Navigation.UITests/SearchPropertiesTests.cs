using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using IC.TimeoutEx;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Diagnostics;
using System.Threading;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class SearchPropertiesTests : IDisposable
    {
        public SearchPropertiesTests()
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
        public void Find_With_Timeout_And_Attributes_Should_Returns_Control_Matching_SearchProperties()
        {
            // Arrange
            IWebElement title = default;

            // Act
            sut.Last.Do(() =>
            {
                title = sut.PomMenu.UITitle.Find(10.s(), ("IsEnabled", "True"));
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void Find_With_Timeout_And_Attributes_Should_Throws_TimeoutException_When_Element_Exists_But_Attributes_Not_Matching()
        {
            // Arrange
            IWebElement title = default;

            // Act
            void actual() => sut.Last.Do(() =>
            {
                title = sut.PomMenu.UITitle.Find(3.s(), ("IsEnabled", "False"));
            });

            // Assert
            var exception = Assert.Throws<OperationCanceledException>(() => actual());
            Assert.Equal("Timeout was reached before attributes match expected values.", exception.Message);
        }

        [Fact]
        public void Find_With_Timeout_Should_Returns_Control_Matching_SearchProperties()
        {
            // Arrange
            IWebElement title = default;

            // Act
            sut.Last.Do(() =>
            {
                title = sut.PomMenu.UITitle.Find(10.s());
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void Find_With_CT_Should_Throws_OperationCanceledException_When_CT_Is_Cancelled()
        {
            // Act
            sut.Last.Do(() =>
            {
                using (var ctsLocal = new CancellationTokenSource(10.s()))
                {
                    ctsLocal.CancelAfter(TimeSpan.Zero);
                    Assert.Throws<OperationCanceledException>(() =>
                        sut.PomMenu.UIBtnNotImplemented.Find(ctsLocal.Token));
                }
            });
        }

        [Fact]
        public void Find_With_Timeout_Should_Throws_TimeoutException_When_Timeout_Is_Reached()
        {
            // Act
            Assert.Throws<TimeoutException>(() =>
                        sut.PomMenu.UIBtnNotImplemented.Find(TimeSpan.Zero));
        }

        [Fact]
        public void Find_With_Timeout_Should_Throws_TimeoutException_On_Timeout()
        {
            // Act
            sut.Last.Do(() =>
            {
                Assert.Throws<TimeoutException>(() =>
                    sut.PomMenu.UIBtnNotImplemented.Find(TimeSpan.Zero));
            });
        }

        [Fact]
        public void SearchProperties_With_DefaultCancellationToken_Should_Returns_Control_Matching()
        {
            // Arrange
            using var defaultTokenSource = new CancellationTokenSource(5.s());
            var titleMenuSearchProp = new SearchProperties<WindowsElement>(
                WindowDriverLocators.AutomationId,
                "TitleMenu",
                sut.RemoteDriver,
                defaultTokenSource.Token);

            // Act
            var titleMenu = titleMenuSearchProp.Get();

            // Assert
            Assert.NotNull(titleMenu);
        }

        [Fact]
        public void Get_Should_Returns_Control_Matching_SearchProperties()
        {
            // Arrange
            IWebElement title = default;

            // Act
            sut.Last.Do(() =>
            {
                title = sut.PomMenu.UITitle.Get();
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void Get_Should_Returns_Null_When_Control_Is_Not_Found()
        {
            // Arrange
            IWebElement title = default;

            // Act
            sut.Last.Do(() =>
            {
                title = sut.PomMenu.UIBtnNotImplemented.Get();
            });

            // Assert
            Assert.Null(title);
        }

        [Fact]
        public void Get_With_Timeout_Should_Returns_Control_Matching_SearchProperties()
        {
            // Arrange
            IWebElement title = default;
            var timeout = 10.s();
            Stopwatch stopwatch = new Stopwatch();
            var maxExpectedElapse = 500.ms();

            // Act
            sut.Last.Do(() =>
            {
                stopwatch.Start();
                title = sut.PomMenu.UITitle.Get(timeout);
                stopwatch.Stop();
            });

            // Assert
            Assert.NotNull(title);
            var diff = timeout.Subtract(timeout.Subtract(stopwatch.Elapsed));
            Assert.True(diff.TotalMilliseconds < maxExpectedElapse.TotalMilliseconds);
        }

        [Fact]
        public void Get_With_CToken_Should_Returns_Control_Matching_SearchProperties()
        {
            // Arrange
            IWebElement title = default;
            var timeout = 10.s();
            Stopwatch stopwatch = new Stopwatch();
            var maxExpectedElapse = 500.ms();

            // Act
            sut.Last.Do(() =>
            {
                stopwatch.Start();
                using (var cts = new CancellationTokenSource())
                {
                    title = sut.PomMenu.UITitle.Get(cts.Token);
                }

                stopwatch.Stop();
            });

            // Assert
            Assert.NotNull(title);
            var diff = timeout.Subtract(timeout.Subtract(stopwatch.Elapsed));
            Assert.True(diff.TotalMilliseconds < maxExpectedElapse.TotalMilliseconds);
        }

        [Fact]
        public void Get_With_Timeout_Should_Returns_Null_When_Control_Is_Not_Found()
        {
            // Arrange
            IWebElement title = default;
            var expectedTimeout = 3.s();
            Stopwatch stopwatch = new Stopwatch();
            var actualElapse = TimeSpan.Zero;
            var expectedMaxElapse = 500.ms();

            // Act
            sut.Last.Do(() =>
            {
                stopwatch.Start();
                title = sut.PomMenu.UIBtnNotImplemented.Get(expectedTimeout);
                stopwatch.Stop();
            });

            // Assert
            Assert.Null(title);
            var diff = actualElapse.Subtract(expectedTimeout);
            Assert.True(diff.TotalMilliseconds < expectedMaxElapse.TotalMilliseconds);
        }

        [Fact]
        public void Get_With_CToken_Should_Returns_Null_When_Control_Is_Not_Found()
        {
            // Arrange
            IWebElement title = default;
            var expectedTimeout = 3.s();
            Stopwatch stopwatch = new Stopwatch();
            var actualElapse = TimeSpan.Zero;
            var expectedMaxElapse = 500.ms();

            // Act
            sut.Last.Do(() =>
            {
                stopwatch.Start();
                using (var cts = new CancellationTokenSource(expectedTimeout))
                {
                    title = sut.PomMenu.UIBtnNotImplemented.Get(cts.Token);
                    stopwatch.Stop();
                }
            });

            // Assert
            Assert.Null(title);
            var diff = actualElapse.Subtract(expectedTimeout);
            Assert.True(diff.TotalMilliseconds < expectedMaxElapse.TotalMilliseconds);
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}