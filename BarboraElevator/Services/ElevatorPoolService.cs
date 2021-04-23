using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Services
{
    public class ElevatorPoolService : IElevatorPoolService
    {
        object locker = new object();
        private readonly ConcurrentDictionary<int, ElevatorModel> freeElevators = new ConcurrentDictionary<int, ElevatorModel>();
        private readonly ConcurrentDictionary<int, ElevatorModel> occupiedElevators = new ConcurrentDictionary<int, ElevatorModel>();

        public ElevatorPoolService()
        {
            for (var i = 0; i < 5; i++)
            {
                freeElevators.TryAdd(i, new ElevatorModel
                {
                    Id = i
                });
            }
        }

        public ElevatorModel TakeClosestElevator(int floor)
        {
            lock (locker)
            {
                var closestElevatorEntry = freeElevators
                    .OrderBy(p => Math.Abs(p.Value.CurrentFloor - floor))
                    .FirstOrDefault();

                if (closestElevatorEntry.Equals(default(KeyValuePair<int, ElevatorModel>)))
                    return null;

                freeElevators.TryRemove(closestElevatorEntry.Key, out var closestElevator);
                occupiedElevators.TryAdd(closestElevatorEntry.Key, closestElevator);
                return closestElevator;
            }
        }

        public void ReleaseElevator(int elevatorId)
        {
            lock (locker)
            {
                occupiedElevators.TryRemove(elevatorId, out var elevator);
                freeElevators.TryAdd(elevatorId, elevator);
            }
        }
    }
}
