using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Extensions.Appium.WindowsDriver;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
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
            wd = sut.WindowsDriver;
        }

        #region Properties

        #region Private

        private IFacade sut;
        private WindowsDriver<WindowsElement> wd;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void ShouldFindBtnBlueView()
        {
            Assert.Equal("BtnOpenBlueView", wd.Get(sut.PomMenu.UIBtnOpenBlueViewParam).GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldNotFindUIBtnFake()
        {
            Assert.Null(wd.Get(sut.PomMenu.UIBtnNotImplementedParam));
        }

        [Fact]
        public void ShouldFindMenuTitle()
        {
            Assert.Equal("TitleMenu", wd.Get(sut.PomMenu.UITitleParam).GetAttribute("AutomationId"));
        }


        [Fact]
        public void ShouldEnterTextInMenuTextBox()
        {
            string expected = "This is a text";
            sut.PomMenu.EnterText(expected);
            Assert.Equal(expected, wd.Get(sut.PomMenu.UITxtBoxImportantMessageParam).Text);
        }

        [Fact]
        public void ShouldOpenBlueView()
        {
            wd.Get(sut.PomMenu.UIBtnOpenBlueViewParam).Click();
            Assert.True(sut.PomBlue.Exists());
        }

        [Fact]
        public void ShouldOpenRedView()
        {
            wd.Get(sut.PomMenu.UIBtnOpenRedViewParam).Click();
            Assert.True(sut.PomRed.Exists());
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}