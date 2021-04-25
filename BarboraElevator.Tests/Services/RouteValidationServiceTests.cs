using BarboraElevator.Services;
using BarboraElevator.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BarboraElevator.Tests.Services
{
    [TestClass]
    public class RouteValidationServiceTests
    {
        private Mock<IBuildingConfigurationService> buildingConfigurationServiceMock;
        private RouteValidationService sut;
        
        [TestInitialize]
        public void Initialize()
        {
            buildingConfigurationServiceMock = new Mock<IBuildingConfigurationService>();

            sut = new RouteValidationService(buildingConfigurationServiceMock.Object);
        }

        [TestMethod]
        [DataRow(1u, 0, false)]
        [DataRow(1u, 1, true)]
        [DataRow(1u, 2, false)]
        [DataRow(10u, 0, false)]
        [DataRow(10u, 1, true)]
        [DataRow(10u, 10, true)]
        [DataRow(10u, 11, false)]
        public void IsFloorNumberCorrect_ReturnsExpectedResults(uint configuredNumberOfFloors, int inputFloorNumber, bool expectedResult)
        {
            // Arrange
            buildingConfigurationServiceMock
                .Setup(x => x.GetNumberOfFloors())
                .Returns(configuredNumberOfFloors);

            // Act
            var result = sut.IsFloorNumberCorrect(inputFloorNumber);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        // one floor
        [DataRow(1u, 0, 0, false)]
        [DataRow(1u, 0, 1, false)]
        [DataRow(1u, 0, 2, false)]
        [DataRow(1u, 1, 2, false)]
        [DataRow(1u, 2, 2, false)]
        [DataRow(1u, 1, 1, true)]
        // ten floors
        [DataRow(10u, 0, 0, false)]
        [DataRow(10u, 0, 1, false)]
        [DataRow(10u, 0, 2, false)]
        [DataRow(10u, 0, 10, false)]
        [DataRow(10u, 0, 11, false)]
        [DataRow(10u, 1, 11, false)]
        [DataRow(10u, 2, 11, false)]
        [DataRow(10u, 10, 11, false)]
        [DataRow(10u, 1, 1, true)]
        [DataRow(10u, 1, 2, true)]
        [DataRow(10u, 1, 10, true)]
        [DataRow(10u, 2, 2, true)]
        [DataRow(10u, 2, 10, true)]
        [DataRow(10u, 10, 10, true)]
        public void IsRouteCorrect_ReturnsExpectedResults(uint configuredNumberOfFloors, int startFloor, int endFloor, bool expectedResult)
        {
            // Arrange
            buildingConfigurationServiceMock
                .Setup(x => x.GetNumberOfFloors())
                .Returns(configuredNumberOfFloors);

            // Act
            var result1 = sut.IsRouteCorrect(startFloor, endFloor);
            var result2 = sut.IsRouteCorrect(endFloor, startFloor);

            // Assert
            Assert.AreEqual(expectedResult, result1);
            Assert.AreEqual(expectedResult, result2);
        }
    }
}
