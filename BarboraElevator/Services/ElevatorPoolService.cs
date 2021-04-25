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
        private readonly IRouteValidationService routeValidationService;

        public ElevatorPoolService(
            IElevatorEventLogService elevatorEventLogService,
            IBuildingConfigurationService buildingConfigurationService,
            IRouteValidationService routeValidationService)
        {
            var numberOfElevators = buildingConfigurationService.GetNumberOfElevators();
            InitializeElevators(numberOfElevators);

            this.elevatorEventLogService = elevatorEventLogService;
            this.routeValidationService = routeValidationService;
        }

        public ReadOnlyElevatorModel GetElevator(int id)
        {
            if (!allElevators.ContainsKey(id))
                throw new ArgumentOutOfRangeException(nameof(id));

            return new ReadOnlyElevatorModel(allElevators[id]);
        }

        public ElevatorModel TakeClosestElevator(int floor)
        {
            if (!routeValidationService.IsFloorNumberCorrect(floor))
                throw new ArgumentOutOfRangeException(nameof(floor));

            lock (locker)
            {
                var orderedElevators = freeElevators
                    .OrderBy(p => Math.Abs(p.Value.CurrentFloor - floor));

                if (!orderedElevators.Any())
                    return null;

                var closestElevatorEntry = orderedElevators.First();

                freeElevators.TryRemove(closestElevatorEntry.Key, out var closestElevator);
                occupiedElevators.TryAdd(closestElevatorEntry.Key, closestElevator);

                elevatorEventLogService.LogEvent(closestElevator, "Called elevator");

                return closestElevator;
            }
        }

        public void ReleaseElevator(int elevatorId)
        {
            if (!allElevators.ContainsKey(elevatorId))
                throw new ArgumentOutOfRangeException(nameof(elevatorId));

            lock (locker)
            {
                var result = true;

                result &= occupiedElevators.TryRemove(elevatorId, out var elevator);
                result &= freeElevators.TryAdd(elevatorId, elevator);

                if (result)
                    elevatorEventLogService.LogEvent(elevator, "Elevator is free");
            }
        }

        private void InitializeElevators(uint numberOfElevators)
        {
            for (var i = 0; i < numberOfElevators; i++)
            {
                var elevator = new ElevatorModel
                {
                    Id = i
                };
                allElevators.Add(elevator.Id, elevator);
                freeElevators.TryAdd(elevator.Id, elevator);
            }
        }
    }
}
