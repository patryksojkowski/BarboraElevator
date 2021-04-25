using BarboraElevator.Model;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorPoolService
    {
        ElevatorModel TakeClosestElevator(int floor);
        void ReleaseElevator(int elevatorId);
        ReadOnlyElevatorModel GetElevator(int id);
    }
}
