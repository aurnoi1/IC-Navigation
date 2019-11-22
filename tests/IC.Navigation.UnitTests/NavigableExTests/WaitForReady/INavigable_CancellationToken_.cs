using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using IC.Navigation.CoreExtensions;
using IC.Navigation.Interfaces;
using IC.Navigation.UnitTests.DataAttributes;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace IC.Navigation.UnitTests.NavigableExTests.WaitForReady
{
    public class INavigable_CancellationToken_
    {
        public class Given_Origin_Exists_And_CancellationToken_is_not_Cancelled_
        {
            [Theory, AutoMoqData]
            public void Then_Should_returns_true(INavigable navigable)
            {
                // Arrange
                var fixture = new Fixture().Customize(new AutoMoqCustomization());
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
                var cancellationToken = cancellationTokenSource.Token;
                Mock.Get(navigable).Setup(x => x.NavigatorSession.WaitForReady(navigable, cancellationToken)).Verifiable();

                // Act
                var actual = navigable.WaitForReady(cancellationToken);

                // Assert
                actual.Should().BeTrue();
            }
        }

        public class Given_Origin_Exists_And_CancellationToken_is_Cancelled_
        {
            [Theory, AutoMoqData]
            public void Then_Should_returns_true(INavigable navigable)
            {
                // Arrange
                var fixture = new Fixture().Customize(new AutoMoqCustomization());
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.Zero);
                var cancellationToken = cancellationTokenSource.Token;
                Mock.Get(navigable).Setup(x => x.NavigatorSession.WaitForReady(navigable, cancellationToken)).Verifiable();

                // Act
                var actual = navigable.WaitForReady(cancellationToken);

                // Assert
                actual.Should().BeFalse();
            }
        }
    }
}