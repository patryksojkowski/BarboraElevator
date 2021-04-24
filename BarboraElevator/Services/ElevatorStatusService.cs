using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Services
{
    public class ElevatorStatusService : IElevatorStatusService
    {
        public string GetStatus(ElevatorModel elevator)
        {
            if (elevator is null)
                throw new ArgumentNullException(nameof(elevator));

            return ElevatorStatus(elevator);
        }

        private string ElevatorStatus(ElevatorModel elevator)
        {
            return $"ElevatorId = {elevator.Id} \n IsMoving = {elevator.IsMoving} \n CurrentFloor {elevator.CurrentFloor} \n Direction : {elevator.MovementDirection}";
        }

    }
}
