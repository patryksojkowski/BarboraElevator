﻿using System;
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
                return BadRequest("No elevator available");

            return Ok("Elevator called");
        }

        [Route("GetStatus")]
        public IActionResult GetElevatorStatus(int elevatorId)
        {
            object status = null;

            try
            {
                var elevator = elevatorPoolService.GetElevator(elevatorId);
                status = elevatorStatusService.GetStatus(elevator);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to get elevator status");
            }

            return Ok(status);
        }

        [Route("GetEventLog")]
        public IActionResult GetElevatorEventLog(int elevatorId)
        {
            string eventLog = null;

            try
            {
                var elevator = elevatorPoolService.GetElevator(elevatorId);
                eventLog = elevatorEventLogService.GetEventLog(elevator);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to get elevator events");
            }

            return Ok(eventLog);
        }
    }
}
