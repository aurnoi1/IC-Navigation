using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium;
using IC.Tests.App.Poms.Appium.Interfaces;
using IC.Tests.App.Poms.Appium.POMs;
using IC.TimeoutEx;
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
    public class NavigationTests : IClassFixture<TestsFixture>, IDisposable
    {
        public NavigationTests(TestsFixture testsFixture)
        {
            appBrowser = testsFixture.AppBrowser;
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            globalCts = new CancellationTokenSource(10.s());
            appBrowser.GlobalCancellationToken = globalCts.Token;
        }

        #region Properties

        #region Private

        private readonly IAppBrowser<WindowsDriver<WindowsElement>> appBrowser;
        private readonly IFixture fixture;
        private CancellationTokenSource globalCts;

        #endregion Private

        #endregion Properties

        #region Methods

        #region Public

        [Fact]
        public void FullExample()
        {
            // Set the GlobalCancellationToken used for the time of the Navigation session.

            using var cts = new CancellationTokenSource(30.s());
            appBrowser.GlobalCancellationToken = cts.Token;
            appBrowser.Last
                .GoTo(appBrowser.PomYellow)
                .Do<PomMenu<WindowsDriver<WindowsElement>>>(() =>
                {
                    // Add a timeout in concurence of GlobalCancellationToken;
                    return appBrowser.PomYellow.OpenMenuByMenuBtn(3.s());
                })
                .GoTo(appBrowser.PomBlue) // Force the path to PomBlue then PomYellow...
                .GoTo(appBrowser.PomYellow) //... to test PomYellow.ActionToOpenViewMenu().
                .GoTo(appBrowser.PomMenu) // Since last was PomBlue, PomYellow.OpenViewMenuByMenuBtn() will be called to go to ViewMenu.
                .Do(() =>
                {
                    appBrowser.PomMenu.EnterText("This is a test");
                })
                .GoTo(appBrowser.PomBlue)
                .Back() // ViewBlue. Becarefull with Domain feature and Back() since Previous may change.
                .GoTo(appBrowser.Historic.ElementAt(1)) // The second element of historic is ViewYellow.
                .GoTo(appBrowser.PomRed)// Auto resolution of path to red with ViewYellowFeat.ResolveBackBtnClick().
                .GoTo(appBrowser.EntryPoint); // The entry point.

            Assert.True(appBrowser.Historic.ElementAt(0).Exists());
        }

        [Fact]
        public void GlobalCancellationToken_Should_Interrupt_Navigation()
        {
            using var testTimeout = new CancellationTokenSource(15.s());
            using var cts = new CancellationTokenSource(5.s());
            appBrowser.GlobalCancellationToken = cts.Token;
            Assert.Throws<OperationCanceledException>(() =>
            {
                while (!testTimeout.IsCancellationRequested)
                {
                    appBrowser.Last
                    .GoTo(appBrowser.PomYellow)
                    .GoTo(appBrowser.Previous);
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

                appBrowser.PomMenu.RegisterObserver(mock);
            }

            // Act
            appBrowser.PomMenu.Exists();

            // Assert
            Assert.NotEmpty(callbackResults);

            // Validate all observers are register.
            var resultObs = callbackResults.Select(x => x.observer).ToList();
            Assert.Equal(observerMocks, resultObs);

            // Validate all observers received the same INavigableEventArgs on WaitForExists().
            callbackResults.ForEach(r => Assert.Same(callbackResults[0].args, r.args));

            // Validate all observers received the same instance of ViewMenu on WaitForExists().
            callbackResults.ForEach(r => Assert.Same(appBrowser.PomMenu, r.observable));
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

                appBrowser.PomMenu.RegisterObserver(mock);
            }

            var expected = observerMocks.ElementAt(2);

            // Act
            appBrowser.PomMenu.UnregisterObserver(expected);
            appBrowser.PomMenu.Exists();
            var registeredObservers = callbackResults.Select(x => x.observer).ToList();

            // Assert
            Assert.NotEmpty(registeredObservers);

            // Only one observer was removed from original list.
            Assert.Single(observerMocks.Except(registeredObservers));

            // Validate all observers are register.
            Assert.DoesNotContain(expected, registeredObservers);
        }

        [Fact]
        public void Do_With_INavigable_Returned_Type_Should_Return_PomMenu()
        {
            using var cts = new CancellationTokenSource(10.s());
            appBrowser.GlobalCancellationToken = cts.Token;
            appBrowser.Last
                .GoTo(appBrowser.PomYellow)
                .Do<INavigable>(() =>
                {
                    // Return the PomMenu which implements INavigable.
                    return appBrowser.PomYellow.OpenMenuByMenuBtn(5.Seconds());
                })
                .GoTo(appBrowser.PomRed);
        }

        [Fact]
        public void GetNavigable_Should_Returns_Same_Instance()
        {
            var instance1 = appBrowser.PomMenu;
            var instance2 = appBrowser.PomMenu;
            Assert.NotNull(instance1);
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void ShouldFindBtnBlueViewByUsageNameInViewMenu()
        {
            WindowsElement match = appBrowser.FindElementByAliasesInLastINavigable("button to open the blue page");
            Assert.Equal("BtnOpenBlueView", match.GetAttribute("AutomationId"));
        }

        [Fact]
        public void ShouldFindMenuViewByNavigation()
        {
            Assert.True(appBrowser.PomMenu.Exists());
        }

        [Fact]
        public void ShouldOpenViewMenuFromYellowViewByOpenViewMenuDirectly()
        {
            using var cts = new CancellationTokenSource(30.s());
            appBrowser.GlobalCancellationToken = cts.Token;
            appBrowser.Last
                .GoTo(appBrowser.PomYellow)
                .Do<PomMenu<WindowsDriver<WindowsElement>>>(() => appBrowser.PomYellow.OpenMenuByMenuBtn(5.s()));

            Assert.True(appBrowser.PomMenu.Exists());
        }

        [Fact]
        public void ShouldEnterTextInMenuTextBoxByDo()
        {
            string expected = "Text enter by a DO action.";
            appBrowser.PomMenu.Do(() => appBrowser.PomMenu.UITxtBoxImportantMessage.Get().SendKeys(expected));
            Assert.Equal(expected, appBrowser.PomMenu.UITxtBoxImportantMessage.Get().Text);
        }

        [Fact]
        public void ShouldGoToBlueView()
        {
            appBrowser.PomMenu.GoTo(appBrowser.PomBlue);
            Assert.True(appBrowser.PomBlue.Exists());
        }

        [Fact]
        public void ShouldHaveViewMenuAsFirstInHistoric()
        {
            appBrowser.PomMenu.GoTo(appBrowser.PomBlue);
            Assert.Equal(typeof(PomMenu<WindowsDriver<WindowsElement>>), appBrowser.Historic.First().GetType());
        }

        [Fact]
        public void ShouldHaveViewBlueAsLastInHistoric()
        {
            appBrowser.PomMenu.GoTo(appBrowser.PomBlue);
            Assert.Equal(typeof(PomBlue<WindowsDriver<WindowsElement>>), appBrowser.Historic.Last().GetType());
        }

        public void Dispose()
        {
            globalCts?.Dispose();
            appBrowser.RemoteDriver.CloseApp();
        }

        #endregion Public

        #endregion Methods
    }
}