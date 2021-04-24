using BarboraElevator.Model;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorPoolService
    {
        public ElevatorModel TakeClosestElevator(int floor);
        void ReleaseElevator(int elevatorId);
        ElevatorModel GetElevator(int id);
    }
}
