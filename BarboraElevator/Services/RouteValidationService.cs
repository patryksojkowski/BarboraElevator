using BarboraElevator.Services.Interfaces;
using BarboraElevator.Constants;

namespace BarboraElevator.Services
{
    public class RouteValidationService : IRouteValidationService
    {
        private readonly IBuildingConfigurationService buildingConfigurationService;

        public RouteValidationService(IBuildingConfigurationService buildingConfigurationService)
        {
            this.buildingConfigurationService = buildingConfigurationService;
        }

        public bool IsFloorNumberCorrect(int floorNumber)
        {
            var numberOfFloors = buildingConfigurationService.GetNumberOfFloors();

            if (floorNumber < Constant.FirstFloorNumber || floorNumber > numberOfFloors)
                return false;

            return true;
        }

        public bool IsRouteCorrect(int start, int end)
        {
            var numberOfFloors = buildingConfigurationService.GetNumberOfFloors();

            if (start < Constant.FirstFloorNumber || start > numberOfFloors)
                return false;

            if (end < Constant.FirstFloorNumber || end > numberOfFloors)
                return false;

            return true;
        }
    }
}
