using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewFeatures.Globals.Domain1;
using IC.Tests.App.UIAccessibility.Appium.ViewNavigables;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Linq;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class NatigationTests : IDisposable
    {
        public NatigationTests()
        {
            sut = new AppiumContext().SUT;
        }

        #region Properties

        #region Private

        private IUIAccess sut;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void FullExample()
        {
            sut.Last
                .GoTo(sut.ViewYellow)
                .Do<ViewMenu>(() =>
                {
                    return sut.ViewYellow.OpenViewMenuByMenuBtn();
                }) // Could be inline: .DoThenFrom<ViewMenu>(() => sut.ViewYellow.OpenViewMenuByMenuBtn());
                .GoTo(sut.ViewBlue) // Force the path to ViewBlue then ViewYellow...
                .GoTo(sut.ViewYellow) //... to test ViewYellowFeat.ActionToOpenViewMenu().
                .GoTo(sut.ViewMenu) // Since last was ViewBlue, ViewYellowFeat.OpenViewMenuByMenuBtn() will be called to go to ViewMenu.
                .Do(() =>
                {
                    sut.ViewMenu.EnterText("This is a test");
                })
                .GoTo(sut.ViewBlue)
                .Log(" of something!") // Domain feature, accessible from any INavigable with path.
                .Back() // ViewBlue. Becarefull with Domain feature and Back() since Previous may change.
                .GoTo(sut.Historic.ElementAt(1)) // The second element of historic is ViewYellow.
                .GoTo(sut.ViewRed)// Auto resolution of path to red with ViewYellowFeat.ResolveBackBtnClick().
                .GoTo(sut.EntryPoint); // The entry point.

            Assert.True(sut.Historic.ElementAt(0).NotifyExistsStatus());
        }

        [Fact]
        public void ShouldFindBtnBlueViewByUsageNameInViewMenu()
        {
            WindowsElement match = sut.FindElementByUsageNameInLastINavigable("button to open the blue view");
            Assert.Equal("BtnOpenBlueView", match.GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldFindMenuViewByNavigation()
        {
            Assert.True(sut.ViewMenu.NotifyExistsStatus());
        }

        [Fact]
        public void ShouldOpenViewMenuFromYellowViewByOpenViewMenuDirectly()
        {
            sut.Last
                .GoTo(sut.ViewYellow)
                .Do<ViewMenu>(() => sut.ViewYellow.OpenViewMenuByMenuBtn());

            Assert.True(sut.ViewMenu.NotifyExistsStatus());
        }

        [Fact]
        public void ShouldEnterTextInMenuTextBoxByDo()
        {
            string expected = "Text enter by a DO action.";
            sut.ViewMenu.Do(() => sut.ViewMenu.UITxtBoxImportantMessage.SendKeys(expected));
            Assert.Equal(expected, sut.ViewMenu.UITxtBoxImportantMessage.Text);
        }

        [Fact]
        public void ShouldGoToBlueView()
        {
            sut.ViewMenu.GoTo(sut.ViewBlue);
            Assert.True(sut.ViewBlue.NotifyExistsStatus());
        }

        [Fact]
        public void ShouldHaveViewMenuAsFirstInHistoric()
        {
            sut.ViewMenu.GoTo(sut.ViewBlue);
            Assert.Equal(typeof(ViewMenu), sut.Historic.First().GetType());
        }

        [Fact]
        public void ShouldHaveViewBlueAsLastInHistoric()
        {
            sut.ViewMenu.GoTo(sut.ViewBlue);
            Assert.Equal(typeof(ViewBlue), sut.Historic.Last().GetType());
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}