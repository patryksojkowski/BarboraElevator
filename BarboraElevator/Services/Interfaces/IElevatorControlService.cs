using BarboraElevator.Model;
using System.Threading.Tasks;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorControlService
    {
        Task LockDoor(ElevatorModel elevator);
        Task UnlockDoor(ElevatorModel elevator);
        Task GoToFloor(ElevatorModel elevator, int targetFloor);
    }
}
