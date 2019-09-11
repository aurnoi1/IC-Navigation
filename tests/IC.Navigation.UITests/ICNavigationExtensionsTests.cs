using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using System;
using System.Collections.Generic;
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
                var title = sut.WindowsDriver.GetWhen(param, "IsEnabled", "True");
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
                var title = sut.WindowsDriver.GetWhen(param, expectedAttribsValues);
                Assert.NotNull(title);
            });
        }

        [Fact]
        public void GetWhen_Should_Returns_Control_When_Many_ValueTuple_Attributes_Are_True()
        {
            sut.PomMenu.Do(() =>
            {
                var param = sut.PomMenu.UITitleParam;
                var title = sut.WindowsDriver.GetWhen(param, ("IsEnabled", "True"), ("IsOffscreen", "False"));
                Assert.NotNull(title);
            });
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}