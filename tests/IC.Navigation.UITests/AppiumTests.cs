using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium;
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
            Assert.Equal("BtnOpenBlueView", sut.PomMenu.UIBtnOpenBlueView.GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldNotFindUIBtnFake()
        {
            Assert.Null(sut.PomMenu.UIBtnNotImplemented);
        }

        [Fact]
        public void ShouldFindMenuTitle()
        {
            Assert.Equal("TitleMenu", sut.PomMenu.UITitle.GetAttribute("AutomationId"));
        }


        [Fact]
        public void ShouldEnterTextInMenuTextBox()
        {
            string expected = "This is a text";
            sut.PomMenu.EnterText(expected);
            Assert.Equal(expected, sut.PomMenu.UITxtBoxImportantMessage.Text);
        }

        [Fact]
        public void ShouldOpenBlueView()
        {
            sut.PomMenu.UIBtnOpenBlueView.Click();
            Assert.True(sut.PomBlue.WaitForExists());
        }

        [Fact]
        public void ShouldOpenRedView()
        {
            sut.PomMenu.UIBtnOpenRedView.Click();
            Assert.True(sut.PomRed.WaitForExists());
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}