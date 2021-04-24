using BarboraElevator.Model.MovementResults;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorRouteService
    {
        ElevatorMovementResult InitiateRoute(int startFloor, int targetFloor);
    }
}
