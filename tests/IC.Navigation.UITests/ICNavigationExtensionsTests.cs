using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            sut.Last.Do(() =>
            {
                var title = sut.WindowsDriver.Get(sut.PomMenu.UITitleParam);
                Assert.NotNull(title);
            });
        }

        [Fact]
        public void GetWhen_Should_Returns_Control_When_Single_Property_Is_True()
        {
            sut.PomMenu.Do(() =>
            {
                var param = sut.PomMenu.UITitleParam;
                var title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), "IsEnabled", "True");
                Assert.NotNull(title);
            });
        }

        [Fact]
        public void GetWhen_Should_Returns_Control_When_Many_Attributes_Are_True()
        {
            sut.PomMenu.Do(() =>
            {
                var param = sut.PomMenu.UITitleParam;
                var expectedAttribsValues = new Dictionary<string, string>();
                expectedAttribsValues.Add("IsEnabled", "True");
                expectedAttribsValues.Add("IsOffscreen", "False");
                var title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), expectedAttribsValues);
                Assert.NotNull(title);
            });
        }

        [Fact]
        public void GetWhen_Should_Returns_Control_When_Many_ValueTuple_Attributes_Are_True()
        {
            sut.PomMenu.Do(() =>
            {
                var param = sut.PomMenu.UITitleParam;
                var title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), ("IsEnabled", "True"), ("IsOffscreen", "False"));
                Assert.NotNull(title);
            });
        }

        [Fact]
        public void GetWhen_Should_Returns_Null_When_Control_With_Dictionnary_Of_Attributes_Is_Not_Found()
        {
            sut.PomMenu.Do(() =>
            {
                var param = sut.PomMenu.UIBtnNotImplementedParam;
                var title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), ("IsEnabled", "True"), ("IsOffscreen", "False"));
                Assert.Null(title);
            });
        }

        [Fact]
        public void GetWhen_Should_Returns_Null_When_Control_With_Many_ValueTuple_Of_Attributes_Is_Not_Found()
        {
            sut.PomMenu.Do(() =>
            {
                var param = sut.PomMenu.UIBtnNotImplementedParam;
                var expectedAttribsValues = new Dictionary<string, string>();
                expectedAttribsValues.Add("IsEnabled", "True");
                expectedAttribsValues.Add("IsOffscreen", "False");
                var title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), expectedAttribsValues);
                Assert.Null(title);
            });
        }

        [Fact]
        public void GetWhen_Should_Returns_Control_With_Single_Property_Is_Not_Found()
        {
            sut.PomMenu.Do(() =>
            {
                var param = sut.PomMenu.UIBtnNotImplementedParam;
                var title = sut.WindowsDriver.GetWhen(param, TimeSpan.FromSeconds(3), "IsEnabled", "True");
                Assert.Null(title);
            });
        }

        [Fact]
        public void GetWhen_Should_Returns_Null_When_Timeout_Even_If_Control_Is_Found()
        {
            // Arrange
            int expectedSeconds = 3;
            var expectedElapse = TimeSpan.FromSeconds(expectedSeconds);
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
                & (actualElapse < TimeSpan.FromSeconds(expectedSeconds + 1));

            Assert.True(isInTimeoutRange);
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}