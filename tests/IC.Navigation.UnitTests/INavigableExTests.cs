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
        public void Exists_Should_Returns_True()
        {
            // Arrange
            var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
            Mock.Get(sut).Setup(x => x.PublishStatus().Exists).Returns(true);

            // Act
            var actual = sut.Exists();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Exists_Should_Returns_False()
        {
            // Arrange
            var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
            Mock.Get(sut).Setup(x => x.PublishStatus().Exists).Returns(false);

            // Act
            var actual = sut.Exists();

            // Assert
            Assert.False(actual);
        }
    }
}