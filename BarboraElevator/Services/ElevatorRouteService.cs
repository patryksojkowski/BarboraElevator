using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System.Threading.Tasks;

namespace BarboraElevator.Services
{
    public class ElevatorRouteService : IElevatorRouteService
    {
        private readonly IElevatorPoolService elevatorPoolService;
        private readonly IElevatorControlService elevatorControlService;

        public ElevatorRouteService(
            IElevatorPoolService elevatorPoolService,
            IElevatorControlService elevatorControlService)
        {
            this.elevatorPoolService = elevatorPoolService;
            this.elevatorControlService = elevatorControlService;
        }

        public ElevatorMovementResult InitiateRoute(int startFloor, int targetFloor)
        {
            if (startFloor == targetFloor)
                return new ElevatorMovementNotNeededResult();

            var elevatorModel = elevatorPoolService.TakeClosestElevator(startFloor);

            if (elevatorModel == null)
                return new ElevatorMovementNotStartedResult();

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

            await elevatorControlService.LockDoor(elevator);

            await elevatorControlService.GoToFloor(elevator, targetFloor);

            await elevatorControlService.UnlockDoor(elevator);
        }
    }
}
