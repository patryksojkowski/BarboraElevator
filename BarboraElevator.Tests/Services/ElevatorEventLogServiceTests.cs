using BarboraElevator.Model;
using BarboraElevator.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarboraElevator.Tests.Services
{
    [TestClass]
    public class ElevatorEventLogServiceTests
    {
        private ElevatorEventLogService sut = new ElevatorEventLogService();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LogEvent_WhenNullProvided_ThrowsException()
        {
            sut.LogEvent(null, string.Empty);
        }

        [TestMethod]
        public void LogEvent_WhenArgumentsProvided_LogsNewEvent()
        {
            // Arrange
            var elevator = new ElevatorModel();
            var subject = "Test purposes subject";

            // Act
            sut.LogEvent(elevator, subject);

            // Assert
            Assert.AreEqual("Test purposes subject", elevator.Events.First().Subject);
            Assert.AreNotEqual(0L, elevator.Events.First().TimeStamp);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEventLog_WhenNullProvided_ThrowsException()
        {
            sut.GetEventLog(null);
        }

        [TestMethod]
        public void GetEventLog_WhenElevatorProvided_ReturnsEventLog()
        {
            // Arrange
            var elevator = new ElevatorModel()
            {
                Events = new List<ElevatorEvent>
                {
                    new ElevatorEvent
                    {
                        Subject = "Test purposes subject",
                        TimeStamp = 1,
                    }
                }
            };

            var readOnlyElevator = new ReadOnlyElevatorModel(elevator);

            // Act
            var result = sut.GetEventLog(readOnlyElevator);

            // Assert
            Assert.IsTrue(result.Contains("1") && result.Contains("Test purposes subject"));
        }
    }
}
