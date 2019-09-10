using IC.Navigation.CoreExtensions;
using IC.Navigation.Extensions.Appium;
using IC.Navigation.UITests.Specflow.Contexts;
using IC.Tests.App.Poms.Appium.Interfaces;
using System;
using Xunit;

namespace IC.Navigation.UITests
{
    [Collection("UITests")]
    public class ICNavigationExtensionsTests : IDisposable
    {
        public ICNavigationExtensionsTests()
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
        public void ShouldFindMenuTitleInPomBlue()
        {
            sut.Last.GoTo(sut.PomBlue).Do(() =>
            {
                var title = sut.WindowsDriver.Get(sut.PomBlue.UILblTitle);
                Assert.NotNull(title);
            });
        }

        public void Dispose()
        {
            sut?.Dispose();
        }

        #endregion Public

        #endregion Methods
    }
}