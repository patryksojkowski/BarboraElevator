using BarboraElevator.Configuration;
using BarboraElevator.Model;
using BarboraElevator.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BarboraElevator.Services
{
    public class ElevatorPoolService : IElevatorPoolService
    {
        private readonly object locker = new object();

        private readonly Dictionary<int, ElevatorModel> allElevators = new Dictionary<int, ElevatorModel>();
        private readonly ConcurrentDictionary<int, ElevatorModel> freeElevators = new ConcurrentDictionary<int, ElevatorModel>();
        private readonly ConcurrentDictionary<int, ElevatorModel> occupiedElevators = new ConcurrentDictionary<int, ElevatorModel>();

        private readonly IElevatorEventLogService elevatorEventLogService;

        public ElevatorPoolService(
            IElevatorEventLogService elevatorEventLogService,
            BuildingConfiguration buildingConfiguration)
        {
            var numberOfElevators = buildingConfiguration.NumberOfElevators;
            for (var i = 0; i < numberOfElevators; i++)
            {
                var elevator = new ElevatorModel
                {
                    Id = i
                };
                allElevators.Add(elevator.Id, elevator);
                freeElevators.TryAdd(elevator.Id, elevator);
            }

            this.elevatorEventLogService = elevatorEventLogService;
        }

        /// <summary>
        /// it could return some kind of lightweight readonly model instead of whole elevator object that could be altered.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ElevatorModel GetElevator(int id)
        {
            if (!allElevators.ContainsKey(id))
                return null;

            return allElevators[id];
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

                elevatorEventLogService.AddNewEvent(closestElevator, "Called elevator");

                return closestElevator;
            }
        }

        public void ReleaseElevator(int elevatorId)
        {
            lock (locker)
            {
                occupiedElevators.TryRemove(elevatorId, out var elevator);
                freeElevators.TryAdd(elevatorId, elevator);

                elevatorEventLogService.AddNewEvent(elevator, "Elevator is free");
            }
        }
    }
}
