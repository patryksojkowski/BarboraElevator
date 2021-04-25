using BarboraElevator.Model;
using BarboraElevator.Model.MovementResults;
using BarboraElevator.Services;
using BarboraElevator.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarboraElevator.Tests.Services
{
    [TestClass]
    public class ElevatorRouteServiceTests
    {
        private Mock<IElevatorPoolService> elevatorPoolServiceMock;
        private Mock<IElevatorControlService> elevatorControlServiceMock;
        private Mock<IRouteValidationService> routeValidationServiceMock;

        private ElevatorRouteService sut;

        [TestInitialize]
        public void Initialize()
        {
            elevatorPoolServiceMock = new Mock<IElevatorPoolService>();

            elevatorControlServiceMock = new Mock<IElevatorControlService>();

            routeValidationServiceMock = new Mock<IRouteValidationService>();

            sut = new ElevatorRouteService(
                elevatorPoolServiceMock.Object,
                elevatorControlServiceMock.Object,
                routeValidationServiceMock.Object);
        }

        [TestMethod]
        public void InitiateRoute_WhenRouteIncorrect_ReturnsMovementNotStartedResult()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsRouteCorrect(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            // Act
            var result = sut.InitiateRoute(1, 2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ElevatorMovementNotStartedResult));
            Assert.AreEqual("Invalid starting or destination floor.", result.Message);
        }

        [TestMethod]
        public void InitiateRoute_WhenRouteIsRedundant_ReturnsMovementNotNeededResult()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsRouteCorrect(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            // Act
            var result = sut.InitiateRoute(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ElevatorMovementNotNeededResult));
        }

        [TestMethod]
        public void InitiateRoute_WhenNoElevatorAvailable_ReturnsMovementNotStartedResult()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsRouteCorrect(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            elevatorPoolServiceMock
                .Setup(x => x.TakeClosestElevator(It.IsAny<int>()))
                .Returns((ElevatorModel)null);

            // Act
            var result = sut.InitiateRoute(1, 2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ElevatorMovementNotStartedResult));
            Assert.AreEqual("No elevators available", result.Message);
        }

        [TestMethod]
        public void InitiateRoute_WhenElevatorAvailable_ReturnsMovementStartedResult()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsRouteCorrect(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            elevatorPoolServiceMock
                .Setup(x => x.TakeClosestElevator(It.IsAny<int>()))
                .Returns(new ElevatorModel());

            // Act
            var result = sut.InitiateRoute(1, 2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ElevatorMovementStartedResult));
        }


    }
}
