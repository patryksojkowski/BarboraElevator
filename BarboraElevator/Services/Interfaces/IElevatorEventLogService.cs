using BarboraElevator.Model;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorEventLogService
    {
        public string GetEventLog(ElevatorModel elevator);
        public void AddNewEvent(ElevatorModel elevator, string subject);
    }
}
