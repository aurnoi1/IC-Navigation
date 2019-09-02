using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using Moq;
using Xunit;

namespace IC.Navigation.UnitTests
{
    public class INavigableExTests
    {
        [Fact]
        public void WaitForExistsShouldReturnTrue()
        {
            // Arrange
            var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
            Mock.Get(sut).Setup(x => x.PublishStatus().Exists).Returns(true);

            // Act
            var actual = sut.WaitForExists();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void WaitForExistsShouldReturnFalse()
        {
            // Arrange
            var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
            Mock.Get(sut).Setup(x => x.PublishStatus().Exists).Returns(false);

            // Act
            var actual = sut.WaitForExists();

            // Assert
            Assert.False(actual);
        }
    }
}