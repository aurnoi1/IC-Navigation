using IC.Navigation.Extensions.Appium;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using System;
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
        public void ContinueWhen_With_CT_Should_Should_Throws_OperationCanceledException_When_AttribueName_Is_Invalid()
        {
            Assert.Throws<OperationCanceledException>(() =>
                sut.WindowsDriver.Get(sut.PomMenu.UITitleParam).ContinueWhen(ct, ("invalidAttribName", "True")));
        }

        [Fact]
        public void ContinueWhen_With_CT_Should_Should_Throws_OperationCanceledException_When_CT_Is_Cancelled()
        {
            using (var ctsLocal = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                ctsLocal.CancelAfter(TimeSpan.Zero);
                Assert.Throws<OperationCanceledException>(() =>
                    sut.WindowsDriver.Get(sut.PomMenu.UITitleParam).ContinueWhen(ctsLocal.Token, ("IsEnabled", "True")));
            }
        }

        [Fact]
        public void ContinueWhen_With_Timeout_Should_Should_Throws_TimeoutException_When_Timeout_Is_Reached()
        {
            Assert.Throws<TimeoutException>(() =>
                sut.WindowsDriver.Get(sut.PomMenu.UITitleParam)
                .ContinueWhen(TimeSpan.Zero, ("IsEnabled", "True")));
        }

        [Fact]
        public void ContinueWhen_With_Timeout_Should_Should_Throws_ArgumentNullException_When_WebElement_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                sut.WindowsDriver.Get(sut.PomMenu.UIBtnNotImplementedParam)
                .ContinueWhen(TimeSpan.Zero, ("IsEnabled", "True")));
        }

        [Fact]
        public void ContinueWhen_With_CT_Should_Should_Throws_ArgumentNullException_When_WebElement_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                sut.WindowsDriver.Get(sut.PomMenu.UIBtnNotImplementedParam)
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