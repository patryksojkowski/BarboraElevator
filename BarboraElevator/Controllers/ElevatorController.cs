using System;
using BarboraElevator.Model.MovementResults;
using BarboraElevator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BarboraElevator.Controllers
{
    [ApiController]
    [Route("elevator")]
    public class ElevatorController : ControllerBase
    {
        private readonly ILogger<ElevatorController> logger;
        private readonly IElevatorRouteService elevatorRouteService;
        private readonly IElevatorPoolService elevatorPoolService;
        private readonly IElevatorEventLogService elevatorEventLogService;
        private readonly IElevatorStatusService elevatorStatusService;

        public ElevatorController(ILogger<ElevatorController> logger,
            IElevatorRouteService elevatorRouteService,
            IElevatorPoolService elevatorPoolService,
            IElevatorEventLogService elevatorEventLogService,
            IElevatorStatusService elevatorStatusService)
        {
            this.logger = logger;
            this.elevatorRouteService = elevatorRouteService;
            this.elevatorPoolService = elevatorPoolService;
            this.elevatorEventLogService = elevatorEventLogService;
            this.elevatorStatusService = elevatorStatusService;
        }

        [Route("call")]
        public IActionResult CallElevator(int start, int end)
        {
            ElevatorMovementResult result;
            try
            {
                result = elevatorRouteService.InitiateRoute(start, end);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to call elevator");
                return BadRequest("Unable to call elevator");
            }

            if (!(result is ElevatorMovementStartedResult startedResult))
                return BadRequest(result?.Message ?? "Unexpected result");

            return Ok(startedResult.Message);
        }

        [Route("status")]
        public IActionResult GetElevatorStatus(int id)
        {
            string status;
            try
            {
                var elevator = elevatorPoolService.GetElevator(id);
                status = elevatorStatusService.GetStatus(elevator);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to get elevator status");
                return BadRequest("Unable to get elevator status");
            }

            return Ok(status);
        }

        [Route("events")]
        public IActionResult GetElevatorEventLog(int id)
        {
            string eventLog;
            try
            {
                var elevator = elevatorPoolService.GetElevator(id);
                eventLog = elevatorEventLogService.GetEventLog(elevator);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to get elevator events");
                return BadRequest("Unable to get elevator events");
            }

            return Ok(eventLog);
        }
    }
}
