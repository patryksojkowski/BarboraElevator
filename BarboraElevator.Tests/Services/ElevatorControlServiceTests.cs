using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using BarboraElevator.Services.Interfaces;
using BarboraElevator.Services;
using System.Threading.Tasks;
using BarboraElevator.Model;

namespace BarboraElevator.Tests.Services
{
    [TestClass]
    public class ElevatorControlServiceTests
    {
        private Mock<IElevatorEventLogService> elevatorEventLogServiceMock;
        private Mock<IRouteValidationService> routeValidationServiceMock;

        private ElevatorControlService sut;

        [TestInitialize]
        public void Initialize()
        {
            elevatorEventLogServiceMock = new Mock<IElevatorEventLogService>();

            routeValidationServiceMock = new Mock<IRouteValidationService>();

            sut = new ElevatorControlService(elevatorEventLogServiceMock.Object, routeValidationServiceMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GoToFloorAsync_WhenNullElevatorProvided_ThrowsException()
        {
            await sut.GoToFloorAsync(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task GoToFloorAsync_WhenNullFloorOutOfRangeProvided_ThrowsException()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(false);

            // Act
            await sut.GoToFloorAsync(new ElevatorModel(), 0);
        }
        
        [TestMethod]
        public async Task GoToFloorAsync_WhenArgumentsAreFine_MovesElevatorUp()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(true);

            var elevator = new ElevatorModel
            {
                CurrentFloor = 1
            };

            // Act
            await sut.GoToFloorAsync(elevator, 2);

            // Assert
            Assert.AreEqual(2, elevator.CurrentFloor);
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task GoToFloorAsync_WhenArgumentsAreFine_MovesElevatorDown()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(true);

            var elevator = new ElevatorModel
            {
                CurrentFloor = 2
            };

            // Act
            await sut.GoToFloorAsync(elevator, 1);

            // Assert
            Assert.AreEqual(1, elevator.CurrentFloor);
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task GoToFloorAsync_WhenTargetFloorIsCurrentFloor_DoesNotMoveElevator()
        {
            // Arrange
            routeValidationServiceMock
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(true);

            var elevator = new ElevatorModel
            {
                CurrentFloor = 2
            };

            // Act
            await sut.GoToFloorAsync(elevator, 2);

            // Assert
            Assert.AreEqual(2, elevator.CurrentFloor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task LockDoorAsync_WhenNullProvided_ThrowsException()
        {
            await sut.LockDoorAsync(null);
        }

        [TestMethod]
        public async Task LockDoorAsync_WhenDoorUnlocked_LocksDoor()
        {
            // Arrange
            var elevator = new ElevatorModel
            {
                IsDoorLocked = false
            };

            // Act
            await sut.LockDoorAsync(elevator);

            // Assert
            Assert.IsTrue(elevator.IsDoorLocked);
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task LockDoorAsync_WhenDoorLocked_LocksDoor()
        {
            // Arrange
            var elevator = new ElevatorModel
            {
                IsDoorLocked = true
            };

            // Act
            await sut.LockDoorAsync(elevator);

            // Assert
            Assert.IsTrue(elevator.IsDoorLocked);
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task UnlockDoorAsync_WhenNullProvided_ThrowsException()
        {
            await sut.UnlockDoorAsync(null);
        }

        [TestMethod]
        public async Task UnlockDoorAsync_WhenDoorLocked_UnlocksDoor()
        {
            // Arrange
            var elevator = new ElevatorModel
            {
                IsDoorLocked = true
            };

            // Act
            await sut.UnlockDoorAsync(elevator);

            // Assert
            Assert.IsFalse(elevator.IsDoorLocked);
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task UnlockDoorAsync_WhenDoorUnlocked_UnlocksDoor()
        {
            // Arrange
            var elevator = new ElevatorModel
            {
                IsDoorLocked = false
            };

            // Act
            await sut.UnlockDoorAsync(elevator);

            // Assert
            Assert.IsFalse(elevator.IsDoorLocked);
            elevatorEventLogServiceMock.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }
    }
}
