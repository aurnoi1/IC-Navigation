using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium;
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
    public class AppiumDriverExTests : IDisposable
    {
        public AppiumDriverExTests()
        {
            sut = new AppiumContext().SUT;
            globalCts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            sut.GlobalCancellationToken = globalCts.Token;
        }

        #region Properties

        #region Private

        private readonly IFacade sut;
        private CancellationTokenSource globalCts;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void Find_With_Timeout_Should_Returns_Control_Matching_SearchParam()
        {
            // Arrange
            WindowsElement title = default;

            // Act
            sut.Last.Do(() =>
            {
                title = sut.WindowsDriver.Find(sut.PomMenu.UITitleParam, TimeSpan.FromSeconds(10));
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
                using (var ctsLocal = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                {
                    ctsLocal.CancelAfter(TimeSpan.Zero);
                    Assert.Throws<OperationCanceledException>(() =>
                        sut.WindowsDriver.Find(sut.PomMenu.UIBtnNotImplemented, ctsLocal.Token));
                }
            });
        }

        [Fact]
        public void Find_With_Timeout_Should_Throws_TimeoutException_When_Timeout_Is_Reached()
        {
            // Act
            Assert.Throws<TimeoutException>(() =>
                        sut.WindowsDriver.Find(sut.PomMenu.UIBtnNotImplemented, TimeSpan.Zero));
        }

        [Fact]
        public void Find_With_Timeout_Should_Throws_TimeoutException_On_Timeout()
        {
            // Act
            sut.Last.Do(() =>
            {
                Assert.Throws<TimeoutException>(() =>
                sut.WindowsDriver.Find(sut.PomMenu.UITitleParam, TimeSpan.Zero));
            });
        }

        [Fact]
        public void Get_Should_Returns_Control_Matching_SearchParam()
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
        public void Get_Should_Returns_Null_When_Control_Is_Not_Found()
        {
            // Arrange
            WindowsElement title = default;

            // Act
            sut.Last.Do(() =>
            {
                title = sut.WindowsDriver.Get(sut.PomMenu.UIBtnNotImplemented);
            });

            // Assert
            Assert.Null(title);
        }

        [Fact]
        public void Get_With_Timeout_Should_Returns_Control_Matching_SearchParam()
        {
            // Arrange
            WindowsElement title = default;
            var timeout = TimeSpan.FromSeconds(10);
            Stopwatch stopwatch = new Stopwatch();
            var maxExpectedElapse = TimeSpan.FromMilliseconds(500);

            // Act
            sut.Last.Do(() =>
            {
                stopwatch.Start();
                title = sut.WindowsDriver.Get(sut.PomMenu.UITitleParam, timeout);
                stopwatch.Stop();
            });

            // Assert
            Assert.NotNull(title);
            var diff = timeout.Subtract(timeout.Subtract(stopwatch.Elapsed));
            Assert.True(diff.TotalMilliseconds < maxExpectedElapse.TotalMilliseconds);
        }

        [Fact]
        public void Get_With_CToken_Should_Returns_Control_Matching_SearchParam()
        {
            // Arrange
            WindowsElement title = default;
            var timeout = TimeSpan.FromSeconds(10);
            Stopwatch stopwatch = new Stopwatch();
            var maxExpectedElapse = TimeSpan.FromMilliseconds(500);

            // Act
            sut.Last.Do(() =>
            {
                stopwatch.Start();
                using (var cts = new CancellationTokenSource())
                {
                    title = sut.WindowsDriver.Get(sut.PomMenu.UITitleParam, cts.Token);
                }

                stopwatch.Stop();
            });

            // Assert
            Assert.NotNull(title);
            var diff = timeout.Subtract(timeout.Subtract(stopwatch.Elapsed));
            Assert.True(diff.TotalMilliseconds < maxExpectedElapse.TotalMilliseconds);
        }

        [Fact]
        public void GetWhen_With_CToken_Should_Returns_Control_When_Many_ValueTuple_Attributes_Are_True()
        {
            // Arrange
            var param = sut.PomMenu.UITitleParam;
            WindowsElement title = default;

            // Act
            sut.PomMenu.Do(() =>
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    title = sut.WindowsDriver.GetWhen(param, cts.Token, ("IsEnabled", "True"), ("IsOffscreen", "False"));
                }
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void GetWhen_With_Timeout_Should_Returns_Control_When_Single_Property_Is_True()
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
        public void GetWhen_With_Timeout_Should_Returns_Control_When_Many_Attributes_Are_True()
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
        public void GetWhen_With_CToken_Should_Returns_Control_When_Single_Property_Is_True()
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
        public void GetWhen_With_CToken_Should_Returns_Control_When_Many_Attributes_Are_True()
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
                    title = sut.WindowsDriver.GetWhen(param, cts.Token, expectedAttribsValues);
                }
            });

            // Assert
            Assert.NotNull(title);
        }

        [Fact]
        public void GetWhen_With_Timeout_Should_Returns_Control_When_Many_ValueTuple_Attributes_Are_True()
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
        public void GetWhen_With_Timeout_Should_Returns_Null_When_Control_With_Dictionnary_Of_Attributes_Is_Not_Found()
        {
            // Arrange
            var param = sut.PomMenu.UIBtnNotImplemented;
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
        public void GetWhen_With_Timeout_Should_Returns_Null_When_Control_With_Many_ValueTuple_Of_Attributes_Is_Not_Found()
        {
            // Arrange
            var param = sut.PomMenu.UIBtnNotImplemented;
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
        public void GetWhen_With_Timeout_Should_Returns_Control_With_Single_Property_Is_Not_Found()
        {
            // Arrange
            var param = sut.PomMenu.UIBtnNotImplemented;
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
        public void Get_With_Timeout_Should_Returns_Null_When_Control_Is_Not_Found()
        {
            // Arrange
            WindowsElement title = default;
            var expectedTimeout = TimeSpan.FromSeconds(3);
            Stopwatch stopwatch = new Stopwatch();
            var actualElapse = TimeSpan.Zero;
            var expectedMaxElapse = TimeSpan.FromMilliseconds(500);

            // Act
            sut.Last.Do(() =>
            {
                stopwatch.Start();
                title = sut.WindowsDriver.Get(sut.PomMenu.UIBtnNotImplemented, expectedTimeout);
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
            WindowsElement title = default;
            var expectedTimeout = TimeSpan.FromSeconds(3);
            Stopwatch stopwatch = new Stopwatch();
            var actualElapse = TimeSpan.Zero;
            var expectedMaxElapse = TimeSpan.FromMilliseconds(500);

            // Act
            sut.Last.Do(() =>
            {
                stopwatch.Start();
                using (var cts = new CancellationTokenSource(expectedTimeout))
                {
                    title = sut.WindowsDriver.Get(sut.PomMenu.UIBtnNotImplemented, cts.Token);
                    stopwatch.Stop();
                }
            });

            // Assert
            Assert.Null(title);
            var diff = actualElapse.Subtract(expectedTimeout);
            Assert.True(diff.TotalMilliseconds < expectedMaxElapse.TotalMilliseconds);
        }

        [Fact]
        public void GetWhen_With_Timeout_Should_Returns_Null_When_Timeout_Even_If_Control_Is_Found()
        {
            // Arrange
            var expectedElapse = TimeSpan.FromSeconds(3);
            var param = sut.PomMenu.UITitleParam;
            Stopwatch stopwatch = new Stopwatch();
            WindowsElement title = default;
            var actualElapse = TimeSpan.Zero;
            var expectedMaxElapse = TimeSpan.FromMilliseconds(500);

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
            var diff = actualElapse.Subtract(expectedElapse);
            Assert.True(diff.TotalMilliseconds < expectedMaxElapse.TotalMilliseconds);
        }

        [Fact]
        public void GetWhen_With_CToken_Should_Returns_Null_When_Cancellation_Is_Requested_Even_If_Control_Is_Found()
        {
            // Arrange
            var expectedElapse = TimeSpan.FromSeconds(3);
            var param = sut.PomMenu.UITitleParam;
            Stopwatch stopwatch = new Stopwatch();
            WindowsElement title = default;
            var actualElapse = TimeSpan.Zero;
            var expectedMaxElapse = TimeSpan.FromMilliseconds(500);

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
            Assert.True(diff.TotalMilliseconds < expectedMaxElapse.TotalMilliseconds);
        }

        [Fact]
        public void GetWhen_With_CToken_Should_Returns_Null_When_Cancellation_Is_Requested_Before_The_End_Of_Timeout()
        {
            // Arrange
            var expectedElapse = TimeSpan.FromSeconds(5);
            var expectedCancellationTimeout = TimeSpan.FromSeconds(2);
            int cancellationTimeoutMS = Convert.ToInt32(expectedCancellationTimeout.TotalMilliseconds);
            var param = sut.PomMenu.UITitleParam;
            Stopwatch stopwatch = new Stopwatch();
            WindowsElement title = default;
            var actualElapse = TimeSpan.Zero;
            var expectedMaxElapse = TimeSpan.FromMilliseconds(500);

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
            Assert.True(diff.TotalMilliseconds < expectedMaxElapse.TotalMilliseconds);
        }

        public void Dispose()
        {
            sut?.Dispose();
            globalCts?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}