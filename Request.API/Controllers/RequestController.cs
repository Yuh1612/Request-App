﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Request.API.Applications.Queries;
using System.Net;
using Request.API.Applications.Commands;
using Microsoft.AspNetCore.Authorization;
using Request.Domain.Entities.Users;

namespace Request.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RequestController> _logger;
        private readonly IRequestQueries _requestQueries;

        public RequestController(IMediator mediator, ILogger<RequestController> logger, IRequestQueries requestQueries)
        {
            _mediator = mediator;
            _logger = logger;
            _requestQueries = requestQueries;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<LeaveRequestResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var result = Guid.TryParse(User.Claims.First(i => i.Type == "id").Value, out var userId);
            return result ? Ok(await _requestQueries.GetLeaveRequestByRequestorId(userId)) : BadRequest();
        }

        [HttpGet]
        [Route("approve")]
        [ProducesResponseType(typeof(List<LeaveRequestResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetApprovedLeaveRequests()
        {
            var result = Guid.TryParse(User.Claims.First(i => i.Type == "id").Value, out var userId);
            return result ? Ok(await _requestQueries.GetLeaveRequestByApproverId(userId)) : BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(LeaveRequestDetail), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLeaveRequest([FromRoute] Guid id)
        {
            var result = Guid.TryParse(User.Claims.First(i => i.Type == "id").Value, out var userId);
            return result ? Ok(await _requestQueries.GetLeaveRequest(id, userId)) : BadRequest();
        }

        [HttpGet]
        [Route("approver/{approverId}")]
        [ProducesResponseType(typeof(List<LeaveRequestResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLeaveRequestsByApproverId([FromRoute] Guid approverId)
        {
            return Ok(await _requestQueries.GetLeaveRequestByApproverId(approverId));
        }

        [HttpGet]
        [Route("requestor/{requestorId}")]
        [ProducesResponseType(typeof(List<LeaveRequestResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLeaveRequestsByRequestorId([FromRoute] Guid requestorId)
        {
            return Ok(await _requestQueries.GetLeaveRequestByRequestorId(requestorId));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestCommand command)
        {
            _logger.LogInformation("----- Sending command: {command})", nameof(command));
            bool commandResult = await _mediator.Send(command);
            return commandResult ? Ok() : BadRequest();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRequest([FromBody] UpdateRequestCommand command)
        {
            bool commandResult = false;
            if (command.Id != Guid.Empty)
            {
                _logger.LogInformation("----- Sending command: {command})", nameof(command));
                commandResult = await _mediator.Send(command);
            }
            return commandResult ? Ok() : BadRequest();
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRequest([FromRoute] DeleteRequestCommand command)
        {
            bool commandResult = false;
            if (command.Id != Guid.Empty)
            {
                _logger.LogInformation("----- Sending command: {command})", nameof(command));
                commandResult = await _mediator.Send(command);
            }
            return commandResult ? Ok() : BadRequest();
        }

        [HttpPost("conduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConductRequest([FromBody] ConductRequestCommand command)
        {
            bool commandResult = false;
            if (command.Id != Guid.Empty)
            {
                _logger.LogInformation("----- Sending command: {command})", nameof(command));
                commandResult = await _mediator.Send(command);
            }
            return commandResult ? Ok() : BadRequest();
        }
    }
}