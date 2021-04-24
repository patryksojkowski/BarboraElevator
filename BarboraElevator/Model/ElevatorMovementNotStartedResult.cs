namespace BarboraElevator.Model
{
    public class ElevatorMovementNotStartedResult : ElevatorMovementResult
    {
        public ElevatorMovementNotStartedResult()
        {
            Message = "Elevator did not start.";
        }
    }
}
