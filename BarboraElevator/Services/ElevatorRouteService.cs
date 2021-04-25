using BarboraElevator.Model;
using BarboraElevator.Model.MovementResults;
using BarboraElevator.Services.Interfaces;
using System.Threading.Tasks;

namespace BarboraElevator.Services
{
    public class ElevatorRouteService : IElevatorRouteService
    {
        private readonly IElevatorPoolService elevatorPoolService;
        private readonly IElevatorControlService elevatorControlService;
        private readonly IRouteValidationService routeValidationService;

        public ElevatorRouteService(
            IElevatorPoolService elevatorPoolService,
            IElevatorControlService elevatorControlService,
            IRouteValidationService routeValidationService)
        {
            this.elevatorPoolService = elevatorPoolService;
            this.elevatorControlService = elevatorControlService;
            this.routeValidationService = routeValidationService;
        }

        public ElevatorMovementResult InitiateRoute(int startFloor, int targetFloor)
        {
            if (!routeValidationService.IsRouteCorrect(startFloor, targetFloor))
            {
                return new ElevatorMovementNotStartedResult
                {
                    Message = "Invalid starting or destination floor."
                };
            }

            if (startFloor == targetFloor)
                return new ElevatorMovementNotNeededResult();

            var elevatorModel = elevatorPoolService.TakeClosestElevator(startFloor);

            if (elevatorModel == null)
            {
                return new ElevatorMovementNotStartedResult
                {
                    Message = "No elevators available"
                };
            }

            Task.Run(() => PerformRoute(elevatorModel, startFloor, targetFloor));

            return new ElevatorMovementStartedResult();
        }

        private async Task PerformRoute(ElevatorModel elevator, int startFloor, int targetFloor)
        {
            await PerformSingleWayRoute(elevator, startFloor);
            await PerformSingleWayRoute(elevator, targetFloor);

            elevatorPoolService.ReleaseElevator(elevator.Id);
        }

        private async Task PerformSingleWayRoute(ElevatorModel elevator, int targetFloor)
        {
            if (elevator.CurrentFloor == targetFloor)
                return;

            await elevatorControlService.LockDoorAsync(elevator);

            await elevatorControlService.GoToFloorAsync(elevator, targetFloor);

            await elevatorControlService.UnlockDoorAsync(elevator);
        }
    }
}
