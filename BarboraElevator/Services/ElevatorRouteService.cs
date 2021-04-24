using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Services
{
    public class ElevatorRouteService : IElevatorRouteService
    {
        private readonly IElevatorPoolService elevatorPoolService;
        private readonly IElevatorEventLogService elevatorEventLogService;

        public ElevatorRouteService(IElevatorPoolService elevatorPoolService,
            IElevatorEventLogService elevatorEventLogService)
        {
            this.elevatorPoolService = elevatorPoolService;
            this.elevatorEventLogService = elevatorEventLogService;
        }

        public ElevatorMovementResult InitiateRoute(int startFloor, int targetFloor)
        {
            if (startFloor == targetFloor)
                return ElevatorMovementResult.NoMovementNeeded;

            var elevatorModel = elevatorPoolService.TakeClosestElevator(startFloor);

            if (elevatorModel == null)
                return ElevatorMovementResult.Failed;

            Task.Run(() => PerformRoute(elevatorModel, startFloor, targetFloor));

            return new ElevatorMovementResult
            {
                MovementInitiatedSuccessfully = true,
            };
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

            await LockDoor(elevator);

            await GoToFloor(elevator, targetFloor);

            await UnlockDoor(elevator);
        }

        private async Task LockDoor(ElevatorModel elevator)
        {
            elevator.IsDoorLocked = true;
            elevatorEventLogService.AddNewEvent(elevator, "Door locked");

            await Task.Delay(2000);
        }

        private async Task GoToFloor(ElevatorModel elevator, int targetFloor)
        {
            var floorsToGo = targetFloor - elevator.CurrentFloor;
            var directionUp = floorsToGo > 0;

            while (elevator.CurrentFloor != targetFloor)
            {
                await Task.Delay(1000);
                var previousFloor = elevator.CurrentFloor;
                elevator.CurrentFloor += directionUp ? 1 : -1;

                elevatorEventLogService.AddNewEvent(elevator, $"Changed floor from {previousFloor} to {elevator.CurrentFloor}");
            }
        }

        private async Task UnlockDoor(ElevatorModel elevator)
        {
            await Task.Delay(2000);
            elevator.IsDoorLocked = false;

            elevatorEventLogService.AddNewEvent(elevator, "Door unlocked");
        }
    }
}
