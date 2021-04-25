using BarboraElevator.Model.Configuration;
using BarboraElevator.Services.Interfaces;

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
