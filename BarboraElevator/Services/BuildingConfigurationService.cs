using BarboraElevator.Configuration;
using BarboraElevator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Services
{
    public class BuildingConfigurationService : IBuildingConfigurationService
    {
        private readonly BuildingConfiguration buildingConfiguration;

        public BuildingConfigurationService(BuildingConfiguration buildingConfiguration)
        {
            this.buildingConfiguration = buildingConfiguration;
        }

        public uint GetNumberOfElevators()
        {
            return buildingConfiguration.NumberOfElevators;
        }

        public uint GetNumberOfFloors()
        {
            return buildingConfiguration.NumberOfFloors;
        }
    }
}
