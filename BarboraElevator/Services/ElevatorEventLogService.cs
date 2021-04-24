using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarboraElevator.Services
{
    public class ElevatorEventLogService : IElevatorEventLogService
    {
        public void AddNewEvent(ElevatorModel elevator, string subject)
        {
            elevator.Events.Add(new ElevatorEvent
            {
                Subject = subject,
                TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()
            });
        }

        public string GetEventLog(ElevatorModel elevator)
        {
            var sb = new StringBuilder();
            var lines =  elevator.Events.Select(x => x.ToString());

            foreach(var line in lines)
            {
                sb.AppendLine(line);
            }

            return sb.ToString();
        }
    }
}
