using MediatR;
using Microsoft.AspNetCore.Mvc;
using Request.API.Applications.Queries;
using System.Net;
using Request.API.Applications.Commands;

namespace Request.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
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

        [HttpPost]
        public async Task<ActionResult<string>> CreateRequest([FromBody] CreateRequestCommand createRequestCommand)
        {
            await _mediator.Send(createRequestCommand);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateRequest([FromBody] UpdateRequestCommand updateRequestCommand)
        {
            await _mediator.Send(updateRequestCommand);
            return Ok();
        }
        [HttpGet]
        [Route("requestor/{requestorId}")]
        [ProducesResponseType(typeof(List<LeaveRequestResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLeaveRequestByRequestorIds([FromRoute] Guid requestorId)
        {
            return Ok(await _requestQueries.GetLeaveRequestByRequestorId(requestorId));
        }

        [HttpDelete("{id}")]
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
        [HttpGet]
        [Route("approver/{approverId}")]
        [ProducesResponseType(typeof(List<LeaveRequestResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLeaveRequests([FromRoute] Guid approverId)
        {
            return Ok(await _requestQueries.GetLeaveRequestByApproverId(approverId));
        }
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(LeaveRequestDetail), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLeaveRequest([FromRoute] Guid id)
        {
            return Ok(await _requestQueries.GetLeaveRequest(id));
        }
    }
}