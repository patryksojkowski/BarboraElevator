using BarboraElevator.Model;
using BarboraElevator.Services;
using BarboraElevator.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BarboraElevator.Tests.Services
{
    [TestClass]
    public class ElevatorPoolServiceTests
    {
        private Mock<IElevatorEventLogService> elevatorEventLogServiceMock;
        private Mock<IBuildingConfigurationService> buildingConfigurationServiceMock;
        private Mock<IRouteValidationService> routeValidationServiceMock;

        private ElevatorPoolService sut;

        [TestInitialize]
        public void Initialize()
        {
            elevatorEventLogServiceMock = new Mock<IElevatorEventLogService>();

            buildingConfigurationServiceMock = new Mock<IBuildingConfigurationService>();
            buildingConfigurationServiceMock
                .Setup(x => x.GetNumberOfElevators())
                .Returns(2);

            routeValidationServiceMock = new Mock<IRouteValidationService>();

            sut = new ElevatorPoolService(
                elevatorEventLogServiceMock.Object,
                buildingConfigurationServiceMock.Object,
                routeValidationServiceMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetElevator_WhenInvalidIdProvided_ThrowsException()
        {
            sut.GetElevator(10);
        }

        [TestMethod]
        public void GetElevator_WhenValidIdProvided_ReturnsReadOnlyElevator()
        {
            // Act
            var result = sut.GetElevator(1);

            // Assert
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeClosestElevator_WhenInvalidFloorProvided_ThrowsException()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(false);

            // Act
            sut.TakeClosestElevator(1);

            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void TakeClosestElevator_WhenNoElevatorAvailable_ReturnsNull()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(true);

            buildingConfigurationServiceMock
                .Setup(x => x.GetNumberOfElevators())
                .Returns(0);

            sut = new ElevatorPoolService(
                elevatorEventLogServiceMock.Object,
                buildingConfigurationServiceMock.Object,
                routeValidationServiceMock.Object);

            // Act
            var result = sut.TakeClosestElevator(1);

            // Assert
            Assert.IsNull(result);
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void TakeClosestElevator_WhenElevatorAvailable_ReturnsElevator()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(true);

            buildingConfigurationServiceMock
                .Setup(x => x.GetNumberOfElevators())
                .Returns(1);

            sut = new ElevatorPoolService(
                elevatorEventLogServiceMock.Object,
                buildingConfigurationServiceMock.Object,
                routeValidationServiceMock.Object);

            // Act
            var result = sut.TakeClosestElevator(1);

            // Assert
            Assert.IsNotNull(result);
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TakeClosestElevator_WhenOneElevatorAvailable_ReturnsElevatorAndThenNull()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(true);

            buildingConfigurationServiceMock
                .Setup(x => x.GetNumberOfElevators())
                .Returns(1);

            sut = new ElevatorPoolService(
                elevatorEventLogServiceMock.Object,
                buildingConfigurationServiceMock.Object,
                routeValidationServiceMock.Object);

            // Act
            var first = sut.TakeClosestElevator(1);
            var second = sut.TakeClosestElevator(1);

            // Assert
            Assert.IsNotNull(first);
            Assert.IsNull(second);
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReleaseElevator_WhenIncorrectIdProvided_ThrowsException()
        {
            // Arrange
            buildingConfigurationServiceMock
                .Setup(x => x.GetNumberOfElevators())
                .Returns(1);

            sut = new ElevatorPoolService(
                elevatorEventLogServiceMock.Object,
                buildingConfigurationServiceMock.Object,
                routeValidationServiceMock.Object);

            // Act
            sut.ReleaseElevator(2);
        }

        [TestMethod]
        public void ReleaseElevator_WhenElevatorIsOccupied_ReleasesElevator()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(true);

            buildingConfigurationServiceMock
                .Setup(x => x.GetNumberOfElevators())
                .Returns(1);

            sut = new ElevatorPoolService(
                elevatorEventLogServiceMock.Object,
                buildingConfigurationServiceMock.Object,
                routeValidationServiceMock.Object);

            // Act
            sut.TakeClosestElevator(1);
            sut.ReleaseElevator(0);

            // Assert
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void ReleaseElevator_WhenElevatorIsNotOccupied_DoesNotReleaseElevator()
        {
            // Arrange
            buildingConfigurationServiceMock
                .Setup(x => x.GetNumberOfElevators())
                .Returns(1);

            sut = new ElevatorPoolService(
                elevatorEventLogServiceMock.Object,
                buildingConfigurationServiceMock.Object,
                routeValidationServiceMock.Object);

            // Act
            sut.ReleaseElevator(0);

            // Assert
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Never);
        }
    }
}
