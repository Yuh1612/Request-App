using MediatR;
using Microsoft.AspNetCore.Mvc;
using Request.API.Applications.Queries;
using System.Net;
using Request.API.Applications.Commands;
using Microsoft.AspNetCore.Authorization;

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
            return Ok(await _requestQueries.GetLeaveRequestByApproverId());
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(LeaveRequestDetail), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLeaveRequest([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _requestQueries.GetLeaveRequest(id));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("approve")]
        [ProducesResponseType(typeof(List<LeaveRequestResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetApprovedLeaveRequests()
        {
            return Ok(await _requestQueries.GetLeaveRequestByRequestorId());
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestCommand command)
        {
            _logger.LogInformation("----- Sending command: {command})", nameof(command));
            bool commandResult = await _mediator.Send(command);
            return commandResult ? Ok() : BadRequest();
        }

        [HttpPut]
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