using BarboraElevator.Model;
using System.Threading.Tasks;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorControlService
    {
        Task LockDoorAsync(ElevatorModel elevator);
        Task UnlockDoorAsync(ElevatorModel elevator);
        Task GoToFloorAsync(ElevatorModel elevator, int targetFloor);
    }
}
