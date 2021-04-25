using BarboraElevator.Helpers;
using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System;
using System.Text;

namespace BarboraElevator.Services
{
    public class ElevatorEventLogService : IElevatorEventLogService
    {
        public void LogEvent(ElevatorModel elevator, string subject)
        {
            if (elevator == null)
                throw new ArgumentNullException();

            elevator.Events.Add(new ElevatorEvent
            {
                Subject = subject,
                TimeStamp = TimeStampHelper.GetCurrentTimeStamp()
            });
        }

        public string GetEventLog(ReadOnlyElevatorModel elevator)
        {
            if (elevator == null)
                throw new ArgumentNullException();

            var sb = new StringBuilder();

            foreach (var ev in elevator.Events)
            {
                sb.AppendLine(ev.ToString());
            }

            return sb.ToString();
        }
    }
}
