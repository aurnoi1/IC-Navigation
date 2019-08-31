using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.Interfaces;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.POMs;
using Moq;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
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
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        #region Properties

        #region Private

        private IFacade sut;
        private IFixture fixture;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void FullExample()
        {
            sut.Last
                .GoTo(sut.ViewYellow)
                .Do<PomMenu>(() =>
                {
                    return sut.ViewYellow.OpenMenuByMenuBtn();
                }) // Could be inline: .DoThenFrom<PomMenu>(() => sut.PomYellow.OpenViewMenuByMenuBtn());
                .GoTo(sut.ViewBlue) // Force the path to PomBlue then PomYellow...
                .GoTo(sut.ViewYellow) //... to test PomYellowFeat.ActionToOpenViewMenu().
                .GoTo(sut.ViewMenu) // Since last was PomBlue, PomYellowFeat.OpenViewMenuByMenuBtn() will be called to go to ViewMenu.
                .Do(() =>
                {
                    sut.ViewMenu.EnterText("This is a test");
                })
                .GoTo(sut.ViewBlue)
                .Back() // ViewBlue. Becarefull with Domain feature and Back() since Previous may change.
                .GoTo(sut.Historic.ElementAt(1)) // The second element of historic is ViewYellow.
                .GoTo(sut.ViewRed)// Auto resolution of path to red with ViewYellowFeat.ResolveBackBtnClick().
                .GoTo(sut.EntryPoint) // The entry point.
                .WaitForExists(ephemeralThinkTime: 5);

            Assert.True(sut.Historic.ElementAt(0).WaitForExists());
        }

        [Fact]
        public void RegisterObserver_Should_Register_Many_Observers()
        {
            // Arrange
            var observerMocks = fixture.CreateMany<INavigableObserver>();
            var callbackResults = new List<(INavigableObserver observer, INavigable observable, INavigableEventArgs args)>();
            foreach (var mock in observerMocks)
            {
                Mock.Get(mock).Setup(x => x.Update(It.IsAny<INavigable>(), It.IsAny<INavigableEventArgs>()))
                    .Callback<INavigable, INavigableEventArgs>((x, y) => callbackResults.Add((mock, x, y)));

                sut.ViewMenu.RegisterObserver(mock);
            }

            // Act
            sut.ViewMenu.WaitForExists();

            // Assert
            Assert.NotEmpty(callbackResults);

            // Validate all observers are register.
            var resultObs = callbackResults.Select(x => x.observer).ToList();
            Assert.Equal(observerMocks, resultObs);

            // Validate all observers received the same INavigableEventArgs on WaitForExists().
            callbackResults.ForEach(r => Assert.Same(callbackResults[0].args, r.args));

            // Validate all observers received the same instance of ViewMenu on WaitForExists().
            callbackResults.ForEach(r => Assert.Same(sut.ViewMenu, r.observable));
        }

        [Fact]
        public void GetView_Should_Returns_Same_Instance()
        {
            var instance1 = sut.ViewMenu;
            var instance2 = sut.ViewMenu;
            Assert.NotNull(instance1);
            Assert.Same(instance1, instance2);
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
            Assert.True(sut.ViewMenu.WaitForExists());
        }

        [Fact]
        public void ShouldOpenViewMenuFromYellowViewByOpenViewMenuDirectly()
        {
            sut.Last
                .GoTo(sut.ViewYellow)
                .Do<PomMenu>(() => sut.ViewYellow.OpenMenuByMenuBtn());

            Assert.True(sut.ViewMenu.WaitForExists());
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
            Assert.True(sut.ViewBlue.WaitForExists());
        }

        [Fact]
        public void ShouldHaveViewMenuAsFirstInHistoric()
        {
            sut.ViewMenu.GoTo(sut.ViewBlue);
            Assert.Equal(typeof(PomMenu), sut.Historic.First().GetType());
        }

        [Fact]
        public void ShouldHaveViewBlueAsLastInHistoric()
        {
            sut.ViewMenu.GoTo(sut.ViewBlue);
            Assert.Equal(typeof(PomBlue), sut.Historic.Last().GetType());
        }



        [Theory, AutoData]
        [InlineAutoData(0)]
        public void ThinkTime_Should_Adjust_Timeout(double thinkTime, TimeSpan timeout)
        {
            sut.ThinkTime = Math.Abs(thinkTime);
            var expected = TimeSpan.FromTicks(timeout.Ticks * Convert.ToInt64(sut.ThinkTime));

            var actual = sut.AdjustTimeout(timeout);

            Assert.Equal(expected, actual);
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}