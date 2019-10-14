using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using Moq;
using System;
using System.Threading;
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

        [Fact]
        public void xxx()
        {
            // Arrange
            using var testTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            using var globalCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            var origin = fixture.Create<INavigable>();
            var sessionMock = new Mock<NavigatorSession>();
            sessionMock.CallBase = true;
            sessionMock.Object.GlobalCancellationToken = globalCts.Token;
            Action<CancellationToken> action = (ct) =>
            {
                while (!testTimeout.IsCancellationRequested)
                {
                    // GlobalCancellationToken is set as ct in Do() extension method.
                    ct.ThrowIfCancellationRequested();
                    Thread.Sleep(150);
                }
            };

            Mock.Get(origin).Setup(x => x.Session).Returns(sessionMock.Object);
            Mock.Get(origin).Setup(x => x.Session.GlobalCancellationToken).Returns(globalCts.Token);
            Mock.Get(origin).Setup(x => x.Session.Do(origin, action, CancellationToken.None)).Returns(origin);

            // Act
            //Thread.Sleep(1000);
            Assert.Throws<OperationCanceledException>(() => origin.Do(action));

            // Assert
            
            Assert.Equal(globalCts.Token, origin.Session.GlobalCancellationToken);
            Assert.True(globalCts.IsCancellationRequested);
        }
    }
}