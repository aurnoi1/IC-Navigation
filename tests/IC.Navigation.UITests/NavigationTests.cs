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
    public class NavigationTests : IDisposable
    {
        public NavigationTests()
        {
            sut = new AppiumContext().SUT;
            wd = sut.WindowsDriver;
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            globalCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            sut.GlobalCancellationToken = globalCts.Token;
        }

        #region Properties

        #region Private

        private readonly IFacade sut;
        private readonly IFixture fixture;
        private readonly WindowsDriver<WindowsElement> wd;
        private CancellationTokenSource globalCts;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void FullExample()
        {
            // Set the GlobalCancellationToken used for the time of the Navigation session.
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            sut.GlobalCancellationToken = cts.Token;
            sut.Last
                .GoTo(sut.PomYellow)
                .Do<PomMenu>(() =>
                {
                    // Add a timeout in concurence of GlobalCancellationToken;
                    return sut.PomYellow.OpenMenuByMenuBtn(TimeSpan.FromSeconds(3));
                })
                .GoTo(sut.PomBlue) // Force the path to PomBlue then PomYellow...
                .GoTo(sut.PomYellow) //... to test PomYellow.ActionToOpenViewMenu().
                .GoTo(sut.PomMenu) // Since last was PomBlue, PomYellow.OpenViewMenuByMenuBtn() will be called to go to ViewMenu.
                .Do(() =>
                {
                    sut.PomMenu.EnterText("This is a test");
                })
                .GoTo(sut.PomBlue)
                .Back() // ViewBlue. Becarefull with Domain feature and Back() since Previous may change.
                .GoTo(sut.Historic.ElementAt(1)) // The second element of historic is ViewYellow.
                .GoTo(sut.PomRed)// Auto resolution of path to red with ViewYellowFeat.ResolveBackBtnClick().
                .GoTo(sut.EntryPoint); // The entry point.

            Assert.True(sut.Historic.ElementAt(0).Exists());
        }

        [Fact]
        public void GlobalCancellationToken_Should_Interrupt_Navigation()
        {
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            sut.GlobalCancellationToken = cts.Token;
            Assert.Throws<OperationCanceledException>(() =>
            {
                while (!testTimeout.IsCancellationRequested)
                {
                    sut.Last
                    .GoTo(sut.PomYellow)
                    .GoTo(sut.Previous);
                }
            });

            Assert.False(testTimeout.IsCancellationRequested, "The test timeout was reached.");
        }

        [Fact]
        public void RegisterObserver_Should_Register_Many_Observers()
        {
            // Arrange
            var observerMocks = fixture.CreateMany<INavigableObserver>();
            var callbackResults = new List<(INavigableObserver observer, INavigable observable, INavigableStatus args)>();
            foreach (var mock in observerMocks)
            {
                Mock.Get(mock).Setup(x => x.Update(It.IsAny<INavigable>(), It.IsAny<INavigableStatus>()))
                    .Callback<INavigable, INavigableStatus>((x, y) => callbackResults.Add((mock, x, y)));

                sut.PomMenu.RegisterObserver(mock);
            }

            // Act
            sut.PomMenu.Exists();

            // Assert
            Assert.NotEmpty(callbackResults);

            // Validate all observers are register.
            var resultObs = callbackResults.Select(x => x.observer).ToList();
            Assert.Equal(observerMocks, resultObs);

            // Validate all observers received the same INavigableEventArgs on WaitForExists().
            callbackResults.ForEach(r => Assert.Same(callbackResults[0].args, r.args));

            // Validate all observers received the same instance of ViewMenu on WaitForExists().
            callbackResults.ForEach(r => Assert.Same(sut.PomMenu, r.observable));
        }

        [Fact]
        public void UnregisterObserver_Should_Unregister_One_Observer()
        {
            // Arrange
            var observerMocks = fixture.CreateMany<INavigableObserver>(5);
            var callbackResults = new List<(INavigableObserver observer, INavigable observable, INavigableStatus args)>();
            foreach (var mock in observerMocks)
            {
                Mock.Get(mock).Setup(x => x.Update(It.IsAny<INavigable>(), It.IsAny<INavigableStatus>()))
                    .Callback<INavigable, INavigableStatus>((x, y) => callbackResults.Add((mock, x, y)));

                sut.PomMenu.RegisterObserver(mock);
            }

            var expected = observerMocks.ElementAt(2);

            // Act
            sut.PomMenu.UnregisterObserver(expected);
            sut.PomMenu.Exists();
            var registeredObservers = callbackResults.Select(x => x.observer).ToList();

            // Assert
            Assert.NotEmpty(registeredObservers);

            // Only one observer was removed from original list.
            Assert.Single(observerMocks.Except(registeredObservers));

            // Validate all observers are register.
            Assert.DoesNotContain(expected, registeredObservers);
        }

        [Fact]
        public void GetView_Should_Returns_Same_Instance()
        {
            var instance1 = sut.PomMenu;
            var instance2 = sut.PomMenu;
            Assert.NotNull(instance1);
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void ShouldFindBtnBlueViewByUsageNameInViewMenu()
        {
            WindowsElement match = sut.FindElementByAliasesInLastINavigable("button to open the blue page");
            Assert.Equal("BtnOpenBlueView", match.GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldFindMenuViewByNavigation()
        {
            Assert.True(sut.PomMenu.Exists());
        }

        [Fact]
        public void ShouldOpenViewMenuFromYellowViewByOpenViewMenuDirectly()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            sut.GlobalCancellationToken = cts.Token;
            sut.Last
                .GoTo(sut.PomYellow)
                .Do<PomMenu>(() => sut.PomYellow.OpenMenuByMenuBtn(TimeSpan.FromSeconds(5)));

            Assert.True(sut.PomMenu.Exists());
        }

        [Fact]
        public void ShouldEnterTextInMenuTextBoxByDo()
        {
            string expected = "Text enter by a DO action.";
            sut.PomMenu.Do(() => wd.Get(sut.PomMenu.UITxtBoxImportantMessageParam).SendKeys(expected));
            Assert.Equal(expected, wd.Get(sut.PomMenu.UITxtBoxImportantMessageParam).Text);
        }

        [Fact]
        public void ShouldGoToBlueView()
        {
            sut.PomMenu.GoTo(sut.PomBlue);
            Assert.True(sut.PomBlue.Exists());
        }

        [Fact]
        public void ShouldHaveViewMenuAsFirstInHistoric()
        {
            sut.PomMenu.GoTo(sut.PomBlue);
            Assert.Equal(typeof(PomMenu), sut.Historic.First().GetType());
        }

        [Fact]
        public void ShouldHaveViewBlueAsLastInHistoric()
        {
            sut.PomMenu.GoTo(sut.PomBlue);
            Assert.Equal(typeof(PomBlue), sut.Historic.Last().GetType());
        }

        public void Dispose()
        {
            sut?.Dispose();
            globalCts?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}