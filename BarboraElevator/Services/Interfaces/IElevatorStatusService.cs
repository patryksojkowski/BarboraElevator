using BarboraElevator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorStatusService
    {
        string GetStatus(ElevatorModel elevator);
    }
}
