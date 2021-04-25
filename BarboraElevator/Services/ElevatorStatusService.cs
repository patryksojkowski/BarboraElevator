using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System;

namespace BarboraElevator.Services
{
    public class ElevatorStatusService : IElevatorStatusService
    {
        public string GetStatus(ReadOnlyElevatorModel elevator)
        {
            if (elevator is null)
                throw new ArgumentNullException(nameof(elevator));

            return ElevatorStatus(elevator);
        }

        private string ElevatorStatus(ReadOnlyElevatorModel elevator)
        {
            return $"ElevatorId = {elevator.Id} \n IsMoving = {elevator.IsMoving} \n CurrentFloor {elevator.CurrentFloor} \n Direction : {elevator.MovementDirection}";
        }
    }
}
