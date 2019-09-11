using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class ICNavigationExtensionsTests : IDisposable
    {
        public ICNavigationExtensionsTests()
        {
            sut = new AppiumContext().SUT;
        }

        #region Properties

        #region Private

        private IFacade sut;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void Get_Should_Returns_Control_Matching_WDSearchParam()
        {
            // Arrange
            WindowsElement title = default;

            // Act
            sut.Last.Do(() =>
            {
                title = sut.WindowsDriver.Get(sut.PomMenu.UITitleParam);
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void GetWhen_CToken_WithTimeout_Should_Returns_Control_When_Single_Property_Is_True()
        {
            // Arrange
            var param = sut.PomMenu.UITitleParam;
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    title = sut.WindowsDriver.GetWhen(param, cts.Token, "IsEnabled", "True");
                }
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void GetWhen_CToken_WithTimeout_Should_Returns_Control_When_Many_Attributes_Are_True()
        {
            // Arrange
            var param = sut.PomMenu.UITitleParam;
            var expectedAttribsValues = new Dictionary<string, string>();
            expectedAttribsValues.Add("IsEnabled", "True");
            expectedAttribsValues.Add("IsOffscreen", "False");
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), expectedAttribsValues);
                }
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void GetWhen_CToken_Should_Returns_Control_When_Many_ValueTuple_Attributes_Are_True()
        {
            // Arrange
            var param = sut.PomMenu.UITitleParam;
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), ("IsEnabled", "True"), ("IsOffscreen", "False"));
                }
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void GetWhen_WithTimeout_Should_Returns_Control_When_Single_Property_Is_True()
        {
            // Arrange
            var param = sut.PomMenu.UITitleParam;
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), "IsEnabled", "True");
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void GetWhen_WithTimeout_Should_Returns_Control_When_Many_Attributes_Are_True()
        {
            // Arrange
            var param = sut.PomMenu.UITitleParam;
            var expectedAttribsValues = new Dictionary<string, string>();
            expectedAttribsValues.Add("IsEnabled", "True");
            expectedAttribsValues.Add("IsOffscreen", "False");
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), expectedAttribsValues);
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void GetWhen_WithTimeout_Should_Returns_Control_When_Many_ValueTuple_Attributes_Are_True()
        {
            // Arrange
            var param = sut.PomMenu.UITitleParam;
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), ("IsEnabled", "True"), ("IsOffscreen", "False"));
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void GetWhen_WithTimeout_Should_Returns_Null_When_Control_With_Dictionnary_Of_Attributes_Is_Not_Found()
        {
            // Arrange
            var param = sut.PomMenu.UIBtnNotImplementedParam;
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), ("IsEnabled", "True"), ("IsOffscreen", "False"));
            });

            // Assert
            Assert.Null(title);
        }

        [Fact]
        public void GetWhen_WithTimeout_Should_Returns_Null_When_Control_With_Many_ValueTuple_Of_Attributes_Is_Not_Found()
        {
            // Arrange
            var param = sut.PomMenu.UIBtnNotImplementedParam;
            var expectedAttribsValues = new Dictionary<string, string>();
            expectedAttribsValues.Add("IsEnabled", "True");
            expectedAttribsValues.Add("IsOffscreen", "False");
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), expectedAttribsValues);
            });

            // Assert
            Assert.Null(title);
        }

        [Fact]
        public void GetWhen_WithTimeout_Should_Returns_Control_With_Single_Property_Is_Not_Found()
        {
            // Arrange
            var param = sut.PomMenu.UIBtnNotImplementedParam;
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), "IsEnabled", "True");
            });

            // Assert
            Assert.Null(title);
        }

        [Fact]
        public void GetWhen_Should_Returns_Null_When_Timeout_Even_If_Control_Is_Found()
        {
            // Arrange
            var expectedElapse = TimeSpan.FromSeconds(3);
            var param = sut.PomMenu.UITitleParam;
            Stopwatch stopwatch = new Stopwatch();
            WindowsElement title = default;
            var actualElapse = TimeSpan.Zero;

            // Act
            sut.PomMenu.Do(() =>
            {
                stopwatch.Start();
                title = sut.WindowsDriver.GetWhen(param, expectedElapse, "IsEnabled", "InvalidValue");
                stopwatch.Stop();
                actualElapse = stopwatch.Elapsed;
            });

            // Assert
            Assert.Null(title);
            bool isInTimeoutRange = (actualElapse > expectedElapse)
                & (actualElapse < expectedElapse.Add(TimeSpan.FromSeconds(1)));

            Assert.True(isInTimeoutRange);
        }

        [Fact]
        public void GetWhen_Should_Returns_Null_When_Cancellation_Is_Requested_Even_If_Control_Is_Found()
        {
            // Arrange
            var expectedElapse = TimeSpan.FromSeconds(3);
            var param = sut.PomMenu.UITitleParam;
            Stopwatch stopwatch = new Stopwatch();
            WindowsElement title = default;
            var actualElapse = TimeSpan.Zero;

            // Act
            sut.PomMenu.Do(() =>
            {
                using (CancellationTokenSource cts = new CancellationTokenSource(expectedElapse))
                {
                    stopwatch.Start();
                    title = sut.WindowsDriver.GetWhen(param, cts.Token, "IsEnabled", "InvalidValue");
                    stopwatch.Stop();
                    actualElapse = stopwatch.Elapsed;
                }
            });

            // Assert
            Assert.Null(title);
            var diff = actualElapse.Subtract(expectedElapse);
            Assert.True(diff.TotalMilliseconds < 500);
        }

        [Fact]
        public void GetWhen_Should_Returns_Null_When_Cancellation_Is_Requested_Before_The_End_Of_Timeout()
        {
            // Arrange
            var expectedElapse = TimeSpan.FromSeconds(5);
            var expectedCancellationTimeout = TimeSpan.FromSeconds(2);
            int cancellationTimeoutMS = Convert.ToInt32(expectedCancellationTimeout.TotalMilliseconds);
            var param = sut.PomMenu.UITitleParam;
            Stopwatch stopwatch = new Stopwatch();
            WindowsElement title = default;
            var actualElapse = TimeSpan.Zero;

            // Act
            sut.PomMenu.Do(() =>
            {
                using (CancellationTokenSource cts = new CancellationTokenSource(expectedElapse))
                {
                    using (var timer = new Timer((x) => cts.Cancel(), null, cancellationTimeoutMS, Timeout.Infinite))
                    {
                        stopwatch.Start();
                        title = sut.WindowsDriver.GetWhen(param, cts.Token, "IsEnabled", "InvalidValue");
                        stopwatch.Stop();
                        actualElapse = stopwatch.Elapsed;
                    }
                }
            });

            // Assert
            Assert.Null(title);
            var diff = actualElapse.Subtract(expectedCancellationTimeout);
            Assert.True(diff.TotalMilliseconds < 500);
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}