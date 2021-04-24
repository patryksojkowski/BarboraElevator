using BarboraElevator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorEventLogService
    {
        public string GetEventLog(ElevatorModel elevator);
        public void AddNewEvent(ElevatorModel elevator, string subject);
    }
}
