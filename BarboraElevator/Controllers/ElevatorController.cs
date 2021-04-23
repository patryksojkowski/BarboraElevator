using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarboraElevator.Model;
using BarboraElevator.Services;
using BarboraElevator.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BarboraElevator.Controllers
{
    [ApiController]
    [Route("elevator")]
    public class ElevatorController : ControllerBase
    {
        private readonly ILogger<ElevatorController> logger;
        private readonly IElevatorRouteService elevatorRouteService;
        private readonly IElevatorPoolService elevatorPoolService;

        public ElevatorController(ILogger<ElevatorController> logger,
            IElevatorRouteService elevatorRouteService,
            IElevatorPoolService elevatorPoolService)
        {
            this.logger = logger;
            this.elevatorRouteService = elevatorRouteService;
            this.elevatorPoolService = elevatorPoolService;
        }

        [Route("CallElevator")]
        public IActionResult CallElevator(int start, int end)
        {
            ElevatorMovementResult result = null;
            try
            {
                result = elevatorRouteService.InitiateRoute(start, end);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to call elevator");
            }

            if (result == null || !result.MovementInitiatedSuccessfully)
                return BadRequest();

            return Ok("Elevator called");
        }

        public IActionResult GetElevatorStatus(int elevatorId)
        {
            var elevatorJson = HttpContext.Session.GetString(elevatorId.ToString());
            var elevator = JsonConvert.DeserializeObject<ElevatorModel>(elevatorJson);

            object status = null;

            try
            {
                // assign elevator status
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to get elevator status");
            }

            return Ok(status);
        }

        public IActionResult GetElevatorEventLog(int elevatorId)
        {
            var elevatorJson = HttpContext.Session.GetString(elevatorId.ToString());
            var elevator = JsonConvert.DeserializeObject<ElevatorModel>(elevatorJson);

            object eventLog = null;

            try
            {
                // assign elevator status
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to get elevator status");
            }

            return Ok(eventLog);
        }
    }
}
