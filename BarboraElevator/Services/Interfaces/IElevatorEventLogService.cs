using BarboraElevator.Model;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorEventLogService
    {
        public string GetEventLog(ReadOnlyElevatorModel elevator);
        public void LogEvent(ElevatorModel elevator, string subject);
    }
}
