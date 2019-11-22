using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Enums;
using IC.Navigation.Interfaces;
using Moq;
using Xunit;

namespace IC.Navigation.UnitTests.NavigableExTests.Exist
{
    public class Default_
    {
        public class Given_Navigable_exist_
        {
            [Fact]
            public void Then_Should_Returns_True()
            {
                // Arrange
                var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
                Mock.Get(sut).Setup(x => x.PublishStatus().Exist)
                    .Returns(new State<bool>(sut, StatesNames.Exist, true));

                // Act
                var actual = sut.Exists();

                // Assert
                Assert.True(actual);
            }
        }

        public class Given_Navigable_do_not_exist_
        {
            [Fact]
            public void Exist_Should_Returns_False()
            {
                // Arrange
                var sut = new Fixture().Customize(new AutoMoqCustomization()).Create<INavigable>();
                Mock.Get(sut).Setup(x => x.PublishStatus().Exist)
                    .Returns(new State<bool>(sut, StatesNames.Exist, false));

                // Act
                var actual = sut.Exists();

                // Assert
                Assert.False(actual);
            }
        }
    }
}