using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.Enums;
using IC.Navigation.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IC.Navigation.UnitTests.NavigatorSessionTests
{
    public class HistoricNotificationTests
    {
        [Fact]
        public void Update_Should_Notify_HistoricObservers()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var observerMocks = fixture.CreateMany<IHistoricObserver>(5);
            INavigable navigable = fixture.Create<INavigable>();
            var status = fixture.Create<INavigableStatus>();
            Mock.Get(status).Setup(x => x.Exist).Returns(new State<bool>(StatesNames.Exist, true));
            var sutMock = new Mock<NavigatorSession>();
            sutMock.CallBase = true;
            var sut = sutMock.Object;

            // callBackOnUpdate is used to monitor the list of observer and the historic they received on Update.
            var callBackOnUpdate = new List<(IHistoricObserver observer, List<INavigable> historic)>();
            foreach (var mock in observerMocks)
            {
                Mock.Get(mock).Setup(x => x.Update(It.IsAny<List<INavigable>>()))
                    .Callback<List<INavigable>>((x) => callBackOnUpdate.Add((mock, x)));

                sut.RegisterObserver(mock);
            }

            // Act
            sut.Update(navigable, status);

            // Assert
            // Validate all observers received the same Historic.
            callBackOnUpdate.ForEach(r => Assert.Same(callBackOnUpdate[0].historic, r.historic));

            // Ensure the historic publish to observers contains the navigable.
            callBackOnUpdate.ForEach(obs => Assert.Contains(navigable, obs.historic));

            // Ensure
            sutMock.Verify(x => x.NotifyHistoricObservers(callBackOnUpdate[0].historic), Times.Once());
        }

        [Fact]
        public void Update_Should_Call_NotifyHistoricObservers_Once()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var observerMocks = fixture.CreateMany<IHistoricObserver>(5);
            INavigable navigable = fixture.Create<INavigable>();
            List<INavigable> historic = new List<INavigable>() { navigable };
            var status = fixture.Create<INavigableStatus>();
            Mock.Get(status).Setup(x => x.Exist).Returns(new State<bool>(StatesNames.Exist, true));
            var sutMock = new Mock<NavigatorSession>();
            sutMock.CallBase = true;
            var sut = sutMock.Object;

            // Act
            sut.Update(navigable, status);

            // Assert
            sutMock.Verify(x => x.NotifyHistoricObservers(historic), Times.Once());
        }

        [Fact]
        public void RegisterObserver_Should_Register_HistoricObservers()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var obs = fixture.Create<IHistoricObserver>();
            var sutMock = new Mock<NavigatorSession>();
            sutMock.CallBase = true;
            var sut = sutMock.Object;

            // Act
            WeakReference<IHistoricObserver> weakReference = sut.RegisterObserver(obs);

            // Assert
            Assert.NotNull(weakReference);
        }

        [Fact]
        public void UnregisterObserver_Should_Unregister_One_Observer()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var observerMocks = fixture.CreateMany<IHistoricObserver>(5);
            var sutMock = new Mock<NavigatorSession>();
            sutMock.CallBase = true;
            var sut = sutMock.Object;
            var navigable = fixture.Create<INavigable>();
            var status = fixture.Create<INavigableStatus>();
            var expectedRemoveObs = observerMocks.ElementAt(3);

            // callBackOnUpdate is used to monitor the count of observer.
            var callBackOnUpdate = new List<IHistoricObserver>();
            foreach (var mock in observerMocks)
            {
                Mock.Get(mock).Setup(x => x.Update(It.IsAny<List<INavigable>>()))
                    .Callback(() => callBackOnUpdate.Add(mock));

                sut.RegisterObserver(mock);
            }

            // Act
            sut.UnregisterObserver(expectedRemoveObs);

            // Update to get the number of observer from callBackOnUpdate.
            sut.Update(navigable, status);

            // Assert
            Assert.DoesNotContain(expectedRemoveObs, callBackOnUpdate);
        }

        [Fact]
        public void NotifyHistoricObservers_Should_Continue_When_No_Observer()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sutMock = new Mock<NavigatorSession>();
            sutMock.CallBase = true;
            var sut = sutMock.Object;
            var historic = fixture.Create<List<INavigable>>();

            // Act
            Exception expected = null;
            Exception actual = null;
            try
            {
                sut.NotifyHistoricObservers(historic);
            }
            catch (Exception e)
            {
                actual = e;
            }

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}