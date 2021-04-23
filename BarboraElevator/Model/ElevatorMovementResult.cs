using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Model
{
    public class ElevatorMovementResult
    {
        public bool MovementInitiatedSuccessfully { get; set; }
        public Task ElevatorMovementJob { get; set; }

        public static ElevatorMovementResult Failed { get; } = new ElevatorMovementResult
        {
            MovementInitiatedSuccessfully = false
        };

        public static ElevatorMovementResult NoMovementNeeded { get; } = new ElevatorMovementResult
        {
            MovementInitiatedSuccessfully = true
        };
    }
}
