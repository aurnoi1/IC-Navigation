using IC.Navigation.CoreExtensions;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using System;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class AppiumTests : IDisposable
    {
        public AppiumTests()
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
        public void ShouldFindBtnBlueView()
        {
            Assert.Equal("BtnOpenBlueView", sut.ViewMenu.UIBtnOpenBlueView.GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldNotFindUIBtnFake()
        {
            Assert.Null(sut.ViewMenu.UIBtnNotImplemented);
        }

        [Fact]
        public void ShouldFindMenuTitle()
        {
            Assert.Equal("TitleMenu", sut.ViewMenu.UITitle.GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldEnterTextInMenuTextBox()
        {
            string expected = "This is a text";
            sut.ViewMenu.EnterText(expected);
            Assert.Equal(expected, sut.ViewMenu.UITxtBoxImportantMessage.Text);
        }

        [Fact]
        public void ShouldOpenBlueView()
        {
            sut.ViewMenu.UIBtnOpenBlueView.Click();
            Assert.True(sut.ViewBlue.WaitForExists());
        }

        [Fact]
        public void ShouldOpenRedView()
        {
            sut.ViewMenu.UIBtnOpenRedView.Click();
            Assert.True(sut.ViewRed.WaitForExists());
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}