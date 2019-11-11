using ApprovalTests;
using ApprovalTests.Reporters;
using IC.Navigation.CoreExtensions;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class AppiumTests : IDisposable
    {
        public AppiumTests()
        {
            sut = new WindowsContext<WindowsDriver<WindowsElement>>().AppBrowser;
            cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            ct = cts.Token;
        }

        #region Properties

        #region Private

        private readonly IAppBrowser<WindowsDriver<WindowsElement>> sut;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken ct;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void ApprovalTest_Should_Returns_Attribute_AutomationId()
        {
            Approvals.Verify(sut.PomMenu.UIBtnOpenBluePage.Get(ct).GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldFindBtnBlueView()
        {
            Assert.Equal("BtnOpenBlueView", sut.PomMenu.UIBtnOpenBluePage.Get(ct).GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldNotFindUIBtnFake()
        {
            Assert.Null(sut.PomMenu.UIBtnNotImplemented.Get());
        }

        [Fact]
        public void ShouldFindMenuTitle()
        {
            Assert.Equal("TitleMenu", sut.PomMenu.UITitle.Get(ct).GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldEnterTextInMenuTextBox()
        {
            string expected = "This is a text";
            sut.PomMenu.EnterText(expected);
            Assert.Equal(expected, sut.PomMenu.UITxtBoxImportantMessage.Get(ct).Text);
        }

        [Fact]
        public void ShouldOpenBlueView()
        {
            sut.PomMenu.UIBtnOpenBluePage.Get(ct).Click();
            Assert.True(sut.PomBlue.Exists());
        }

        [Fact]
        public void ShouldOpenRedView()
        {
            sut.PomMenu.UIBtnOpenRedPage.Get(ct).Click();
            Assert.True(sut.PomRed.Exists());
        }

        public void Dispose()
        {
            sut.RemoteDriver.CloseApp();
            cts?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}