using IC.Navigation.Extensions.Appium;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class WebElementExTests : IDisposable
    {
        public WebElementExTests()
        {
            sut = new AppiumContext().SUT;
            cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            ct = cts.Token;
        }

        #region Properties

        #region Private

        private IFacade sut;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken ct;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void ContinueWhen_With_Tuple_And_CT_Should_Chain_Once_Attribute_Match_Expected_Value()
        {
            // Arrange
            var expected = true;

            // Act
            var actual = sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .ContinueWhen(ct, ("IsEnabled", "True"))
                .Enabled;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ContinueWhen_With_Tuple_And_Timeout_Should_Chain_Once_Attribute_Match_Expected_Value()
        {
            // Arrange
            var expected = true;

            // Act
            var actual = sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .ContinueWhen(TimeSpan.FromSeconds(1), ("IsEnabled", "True"))
                .Enabled;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ContinueWhen_With_Dic_And_CT_Should_Chain_Once_Attribute_Match_Expected_Value()
        {
            // Arrange
            var expected = true;

            // Act
            var actual = sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .ContinueWhen(ct, new Dictionary<string, string>() { { "IsEnabled", "True" } })
                .Enabled;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ContinueWhen_With_Dic_And_Timeout_Should_Chain_Once_Attribute_Match_Expected_Value()
        {
            // Arrange
            var expected = true;

            // Act
            var actual = sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .ContinueWhen(TimeSpan.FromSeconds(1), new Dictionary<string, string>() { { "IsEnabled", "True" } })
                .Enabled;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ContinueWhen_With_One_Attrib_And_CT_Should_Chain_Once_Attribute_Match_Expected_Value()
        {
            // Arrange
            var expected = true;

            // Act
            var actual = sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .ContinueWhen(ct, "IsEnabled", "True")
                .Enabled;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ContinueWhen_With_One_Attrib_And_Timeout_Should_Chain_Once_Attribute_Match_Expected_Value()
        {
            // Arrange
            var expected = true;

            // Act
            var actual = sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .ContinueWhen(TimeSpan.FromSeconds(1), "IsEnabled", "True")
                .Enabled;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WaitUntil_With_One_Attrib_And_CT_Should_Returns_True_When_Attribute_Match_Expected_Value()
        {
            Assert.True(
                sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .WaitUntil(ct, "IsEnabled", "True"));
        }

        [Fact]
        public void WaitUntil_With_One_Attrib_And_Timeout_Should_Returns_True_When_Attribute_Match_Expected_Value()
        {
            Assert.True(
                sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .WaitUntil(TimeSpan.FromSeconds(1), "IsEnabled", "True"));
        }

        [Fact]
        public void WaitUntil_With_Tuple_And_CT_Should_Returns_True_When_Attribute_Match_Expected_Value()
        {
            Assert.True(
                sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .WaitUntil(ct, ("IsEnabled", "True")));
        }

        [Fact]
        public void WaitUntil_With_Tuple_And_Timeout_Should_Returns_True_When_Attribute_Match_Expected_Value()
        {
            Assert.True(
                sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .WaitUntil(TimeSpan.FromSeconds(1), ("IsEnabled", "True")));
        }

        [Fact]
        public void WaitUntil_With_Tuple_And_CT_Should_Accept_Multiple_Value()
        {
            Assert.True(
                sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .WaitUntil(ct, ("IsEnabled", "True"), ("IsOffscreen", "False")));
        }

        [Fact]
        public void WaitUntil_With_Tuple_And_Timeout_Should_Accept_Multiple_Value()
        {
            Assert.True(
                sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .WaitUntil(TimeSpan.FromSeconds(1), ("IsEnabled", "True"), ("IsOffscreen", "False")));
        }

        [Fact]
        public void WaitUntil_With_Dic_And_CT_Should_Returns_True_When_Attribute_Match_Expected_Value()
        {
            Assert.True(
                sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .WaitUntil(ct, new Dictionary<string, string>() { { "IsEnabled", "True" } }));
        }

        [Fact]
        public void WaitUntil_With_Dic_And_Timeout_Should_Returns_True_When_Attribute_Match_Expected_Value()
        {
            Assert.True(
                sut.WindowsDriver
                .Get(sut.PomMenu.UITitle)
                .WaitUntil(TimeSpan.FromSeconds(1), new Dictionary<string, string>() { { "IsEnabled", "True" } }));
        }

        [Fact]
        public void ContinueWhen_With_CT_Should_Throws_OperationCanceledException_When_AttribueName_Is_Invalid()
        {
            Assert.Throws<OperationCanceledException>(() =>
                sut.WindowsDriver.Get(sut.PomMenu.UITitle).ContinueWhen(ct, ("invalidAttribName", "True")));
        }

        [Fact]
        public void ContinueWhen_With_CT_Should_Throws_OperationCanceledException_When_CT_Is_Cancelled()
        {
            using (var ctsLocal = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                ctsLocal.CancelAfter(TimeSpan.Zero);
                Assert.Throws<OperationCanceledException>(() =>
                    sut.WindowsDriver.Get(sut.PomMenu.UITitle).ContinueWhen(ctsLocal.Token, ("IsEnabled", "False")));
            }
        }

        [Fact]
        public void ContinueWhen_With_Timeout_Should_Throws_TimeoutException_When_Timeout_Is_Reached()
        {
            Assert.Throws<TimeoutException>(() =>
                sut.WindowsDriver.Get(sut.PomMenu.UITitle)
                .ContinueWhen(TimeSpan.Zero, ("IsEnabled", "False")));
        }

        [Fact]
        public void ContinueWhen_With_Timeout_Should_Throws_ArgumentNullException_When_WebElement_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                sut.WindowsDriver.Get(sut.PomMenu.UIBtnNotImplemented)
                .ContinueWhen(TimeSpan.Zero, ("IsEnabled", "True")));
        }

        [Fact]
        public void ContinueWhen_With_CT_Should_Throws_ArgumentNullException_When_WebElement_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                sut.WindowsDriver.Get(sut.PomMenu.UIBtnNotImplemented)
                .ContinueWhen(ct, ("IsEnabled", "True")));
        }

        public void Dispose()
        {
            sut?.Dispose();
            cts?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}