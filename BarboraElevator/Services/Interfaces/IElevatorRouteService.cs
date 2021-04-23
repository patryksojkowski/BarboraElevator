using BarboraElevator.Model;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorRouteService
    {
        ElevatorMovementResult InitiateRoute(int startFloor, int targetFloor);
    }
}
