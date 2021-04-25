using BarboraElevator.Constants;
using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace BarboraElevator.Services
{
    public class ElevatorControlService : IElevatorControlService
    {
        private readonly IElevatorEventLogService elevatorEventLogService;
        private readonly IRouteValidationService routeValidationService;

        public ElevatorControlService(IElevatorEventLogService elevatorEventLogService, IRouteValidationService routeValidationService)
        {
            this.elevatorEventLogService = elevatorEventLogService;
            this.routeValidationService = routeValidationService;
        }

        public Task GoToFloorAsync(ElevatorModel elevator, int targetFloor)
        {
            if (elevator == null)
                throw new ArgumentNullException(nameof(elevator));

            if (!routeValidationService.IsFloorNumberCorrect(targetFloor))
                throw new ArgumentOutOfRangeException(nameof(targetFloor));

            return GoToFloorInternalAsync(elevator, targetFloor);
        }

        public Task LockDoorAsync(ElevatorModel elevator)
        {
            if (elevator == null)
                throw new ArgumentNullException(nameof(elevator));

            return LockDoorInternalAsync(elevator);
        }

        public Task UnlockDoorAsync(ElevatorModel elevator)
        {
            if (elevator == null)
                throw new ArgumentNullException(nameof(elevator));

            return UnlockDoorInternalAsync(elevator);
        }

        private async Task GoToFloorInternalAsync(ElevatorModel elevator, int targetFloor)
        {
            var floorsToGo = targetFloor - elevator.CurrentFloor;
            var isGoingUp = floorsToGo > 0;
            elevator.IsMoving = true;
            elevator.IsGoingUp = isGoingUp;

            while (elevator.CurrentFloor != targetFloor)
            {
                await Task.Delay(Constant.MovementPerFloorTimeInMiliseconds);
                var previousFloor = elevator.CurrentFloor;
                elevator.CurrentFloor += isGoingUp ? 1 : -1;

                elevatorEventLogService.LogEvent(elevator, $"Changed floor from {previousFloor} to {elevator.CurrentFloor}");
            }

            elevator.IsMoving = false;
        }

        private async Task LockDoorInternalAsync(ElevatorModel elevator)
        {
            elevator.IsDoorLocked = true;
            elevatorEventLogService.LogEvent(elevator, "Door locked");

            await Task.Delay(Constant.DoorOperationTimeInMiliseconds);
        }

        private async Task UnlockDoorInternalAsync(ElevatorModel elevator)
        {
            await Task.Delay(Constant.DoorOperationTimeInMiliseconds);
            elevator.IsDoorLocked = false;

            elevatorEventLogService.LogEvent(elevator, "Door unlocked");
        }
    }
}
