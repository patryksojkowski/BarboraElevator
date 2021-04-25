using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using BarboraElevator.Controllers;
using Microsoft.Extensions.Logging;
using BarboraElevator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BarboraElevator.Model.MovementResults;
using BarboraElevator.Model;

namespace BarboraElevator.Tests.Controllers
{
    [TestClass]
    public class ElevatorControllerTests
    {
        private string testPurposesExceptionMessage = "test purposes exception";

        private Mock<ILogger<ElevatorController>> loggerMock;
        private Mock<IElevatorRouteService> elevatorRouteServiceMock;
        private Mock<IElevatorPoolService> elevatorPoolServiceMock;
        private Mock<IElevatorEventLogService> elevatorEventLogServiceMock;
        private Mock<IElevatorStatusService> elevatorStatusServiceMock;

        private ElevatorController sut;

        [TestInitialize]
        public void Intialize()
        {
            loggerMock = new Mock<ILogger<ElevatorController>>();

            elevatorRouteServiceMock = new Mock<IElevatorRouteService>();

            elevatorPoolServiceMock = new Mock<IElevatorPoolService>();

            elevatorEventLogServiceMock = new Mock<IElevatorEventLogService>();

            elevatorStatusServiceMock = new Mock<IElevatorStatusService>();

            sut = new ElevatorController(
                loggerMock.Object,
                elevatorRouteServiceMock.Object,
                elevatorPoolServiceMock.Object,
                elevatorEventLogServiceMock.Object,
                elevatorStatusServiceMock.Object);
        }

        [TestMethod]
        public void CallElevator_ReturnsBadRequest_WhenInitiateRouteThrows()
        {
            // Arrange
            elevatorRouteServiceMock
                .Setup(x => x.InitiateRoute(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new InvalidOperationException(testPurposesExceptionMessage));

            // Act
            var result = sut.CallElevator(1, 2) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CallElevator_ReturnsBadRequest_WhenInitiateRouteResultIsNotStarted()
        {
            // Arrange
            elevatorRouteServiceMock
                .Setup(x => x.InitiateRoute(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new ElevatorMovementNotStartedResult());

            // Act
            var result = sut.CallElevator(1, 2) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CallElevator_ReturnsBadRequest_WhenInitiateRouteResultIsNotNeeded()
        {
            // Arrange
            elevatorRouteServiceMock
                .Setup(x => x.InitiateRoute(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new ElevatorMovementNotNeededResult());

            // Act
            var result = sut.CallElevator(1, 2) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CallElevator_ReturnsOk_WhenInitiateRouteResultIsStarted()
        {
            // Arrange
            elevatorRouteServiceMock
                .Setup(x => x.InitiateRoute(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new ElevatorMovementStartedResult());

            // Act
            var result = sut.CallElevator(1, 2) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetElevatorStatus_ReturnsBadRequest_WhenPoolServiceThrows()
        {
            // Arrange
            elevatorPoolServiceMock
                .Setup(x => x.GetElevator(It.IsAny<int>()))
                .Throws(new InvalidOperationException(testPurposesExceptionMessage));

            // Act
            var result = sut.GetElevatorStatus(1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetElevatorStatus_ReturnsBadRequest_WhenStatusServiceThrows()
        {
            // Arrange
            elevatorStatusServiceMock
                .Setup(x => x.GetStatus(It.IsAny<ReadOnlyElevatorModel>()))
                .Throws(new InvalidOperationException(testPurposesExceptionMessage));

            // Act
            var result = sut.GetElevatorStatus(1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetElevatorStatus_ReturnsOk_WhenStatusServiceReturns()
        {
            // Arrange
            elevatorStatusServiceMock
                .Setup(x => x.GetStatus(It.IsAny<ReadOnlyElevatorModel>()))
                .Returns("status string");

            // Act
            var result = sut.GetElevatorStatus(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("status string", result.Value);
        }

        [TestMethod]
        public void GetElevatorEventLog_ReturnsBadRequest_WhenPoolServiceThrows()
        {
            // Arrange
            elevatorPoolServiceMock
                .Setup(x => x.GetElevator(It.IsAny<int>()))
                .Throws(new InvalidOperationException(testPurposesExceptionMessage));

            // Act
            var result = sut.GetElevatorEventLog(1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetElevatorEventLog_ReturnsBadRequest_WhenEventServiceThrows()
        {
            // Arrange
            elevatorEventLogServiceMock
                .Setup(x => x.GetEventLog(It.IsAny<ReadOnlyElevatorModel>()))
                .Throws(new InvalidOperationException(testPurposesExceptionMessage));

            // Act
            var result = sut.GetElevatorEventLog(1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetElevatorEventLog_ReturnsOk_WhenEventServiceReturns()
        {
            // Arrange
            elevatorEventLogServiceMock
                .Setup(x => x.GetEventLog(It.IsAny<ReadOnlyElevatorModel>()))
                .Returns("event log");

            // Act
            var result = sut.GetElevatorEventLog(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("event log", result.Value);
        }
    }
}
