using BarboraElevator.Constants;
using System.Collections.Generic;

namespace BarboraElevator.Model
{
    public class ReadOnlyElevatorModel
    {
        public ReadOnlyElevatorModel(ElevatorModel elevator)
        {
            Id = elevator.Id;
            IsDoorLocked = elevator.IsDoorLocked;
            IsMoving = elevator.IsMoving;
            IsGoingUp = elevator.IsGoingUp;
            CurrentFloor = elevator.CurrentFloor;
            Events = elevator.Events;
        }

        public int Id { get; }
        public bool IsDoorLocked { get; }
        public bool IsMoving { get; }
        public bool IsGoingUp { get; }
        public int CurrentFloor { get; } = Constant.FirstFloorNumber;
        public IList<ElevatorEvent> Events { get; } = new List<ElevatorEvent>();
        public string MovementDirection => IsMoving ? (IsGoingUp ? "Up" : "Down") : "None";
    }
}
