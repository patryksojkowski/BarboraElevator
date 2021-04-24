using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Services
{
    public class ElevatorControlService : IElevatorControlService
    {
        private readonly IElevatorEventLogService elevatorEventLogService;

        public ElevatorControlService(IElevatorEventLogService elevatorEventLogService)
        {
            this.elevatorEventLogService = elevatorEventLogService;
        }

        public async Task GoToFloor(ElevatorModel elevator, int targetFloor)
        {
            var floorsToGo = targetFloor - elevator.CurrentFloor;
            var isGoingUp = floorsToGo > 0;
            elevator.IsMoving = true;
            elevator.IsGoingUp = isGoingUp;

            while (elevator.CurrentFloor != targetFloor)
            {
                await Task.Delay(1000);
                var previousFloor = elevator.CurrentFloor;
                elevator.CurrentFloor += isGoingUp ? 1 : -1;

                elevatorEventLogService.AddNewEvent(elevator, $"Changed floor from {previousFloor} to {elevator.CurrentFloor}");
            }

            elevator.IsMoving = false;
        }

        public async Task LockDoor(ElevatorModel elevator)
        {
            elevator.IsDoorLocked = true;
            elevatorEventLogService.AddNewEvent(elevator, "Door locked");

            await Task.Delay(2000);
        }

        public async Task UnlockDoor(ElevatorModel elevator)
        {
            await Task.Delay(2000);
            elevator.IsDoorLocked = false;

            elevatorEventLogService.AddNewEvent(elevator, "Door unlocked");
        }
    }
}
