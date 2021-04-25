using BarboraElevator.Model.Configuration;
using BarboraElevator.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarboraElevator.Tests.Services
{
    [TestClass]
    public class BuildingConfigurationServiceTests
    {
        private BuildingConfiguration configuration;
        private BuildingConfigurationService sut;

        [TestInitialize]
        public void Initialize()
        {
            configuration = new BuildingConfiguration
            {
                NumberOfElevators = 5u,
                NumberOfFloors = 10u
            };

            sut = new BuildingConfigurationService(configuration);
        }

        [TestMethod]
        public void GetNumberOfElevators_ReturnsCorrectValue()
        {
            // Act
            var result = sut.GetNumberOfElevators();

            // Assert
            Assert.AreEqual(5u, result);
        }

        [TestMethod]
        public void GetNumberOfFloors_ReturnsCorrectValue()
        {
            // Act
            var result = sut.GetNumberOfFloors();

            // Assert
            Assert.AreEqual(10u, result);
        }
    }
}
