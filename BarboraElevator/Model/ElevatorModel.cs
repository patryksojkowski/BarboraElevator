using System.Collections.Generic;

namespace BarboraElevator.Model
{
    public class ElevatorModel
    {
        public int Id { get; set; }
        public bool IsDoorLocked { get; set; }
        public bool IsMoving { get; set; }
        public bool IsGoingUp { get; set; }
        public int CurrentFloor { get; set; }
        public IList<ElevatorEvent> Events { get; set; } = new List<ElevatorEvent>();
        public string MovementDirection => IsMoving ? (IsGoingUp ? "Up" : "Down") : "None";
    }
}
