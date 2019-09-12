using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.Interfaces;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using IC.Tests.App.Poms.Appium.POMs;
using Moq;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class FacadeTests
    {
        public FacadeTests()
        {
            appContext = new AppiumContext();
        }

        #region Properties

        #region Private

        private readonly AppiumContext appContext;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void WaitForEntryPoints_With_CToken_Should_Returns_Found_EntryPoint()
        {
            var expected = typeof(PomMenu);
            INavigable actual = null;
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            using (var sut = appContext.GetFacade())
            {
               actual = sut.WaitForEntryPoints(cts.Token);
            }

            Assert.Equal(expected, actual.GetType());
        }

        [Fact]
        public void WaitForEntryPoints_With_CToken_Should_Throw_OperationCanceledException_On_Timeout()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1)))
            using (var sut = appContext.GetFacade())
            {
                Assert.Throws<OperationCanceledException>(() => sut.WaitForEntryPoints(cts.Token));
            }
        }

        [Fact]
        public void WaitForEntryPoints_With_Timeout_Should_Returns_Found_EntryPoint()
        {
            var expected = typeof(PomMenu);
            INavigable actual = null;
            using (var sut = appContext.GetFacade())
            {
                actual = sut.WaitForEntryPoints(TimeSpan.FromSeconds(10));
            }

            Assert.Equal(expected, actual.GetType());
        }

        [Fact]
        public void WaitForEntryPoints_With_Timeout_Should_Throw_TimeoutException_On_Timeout()
        {
            using (var sut = appContext.GetFacade())
            {
                Assert.Throws<TimeoutException>(() => sut.WaitForEntryPoints(TimeSpan.FromMilliseconds(1)));
            }
        }

        #endregion Public

        #endregion Methods
    }
}