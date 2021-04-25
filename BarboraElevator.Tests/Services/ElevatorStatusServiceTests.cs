using BarboraElevator.Model;
using BarboraElevator.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BarboraElevator.Tests.Services
{
    [TestClass]
    public class ElevatorStatusServiceTests
    {
        private ElevatorStatusService sut = new ElevatorStatusService();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetStatus_WhenNullProvided_ThrowsException()
        {
            sut.GetStatus(null);
        }

        [TestMethod]
        public void GetStatus_WhenElevatorProvided_ShouldReturnStatus()
        {
            // Arrange
            var elevator = new ElevatorModel();
            var readOnlyElevator = new ReadOnlyElevatorModel(elevator);

            // Act
            var result = sut.GetStatus(readOnlyElevator);

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }
    }
}
