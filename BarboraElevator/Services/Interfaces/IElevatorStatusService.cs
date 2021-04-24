using BarboraElevator.Model;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorStatusService
    {
        string GetStatus(ElevatorModel elevator);
    }
}
