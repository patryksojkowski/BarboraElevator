using BarboraElevator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Services.Interfaces
{
    public interface IElevatorPoolService
    {
        public ElevatorModel TakeClosestElevator(int floor);
        void ReleaseElevator(int elevatorId);
    }
}
