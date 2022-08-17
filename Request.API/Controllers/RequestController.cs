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
        [Route("{userId}")]
        [ProducesResponseType(typeof(IEnumerator<LeaveRequestReponse>), (int)HttpStatusCode.OK)]
        public IActionResult GetLeaveRequests([FromRoute] Guid userId)
        {
            return Ok(_requestQueries.GetLeaveRequestByUserId(userId.ToString()).Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest([FromRoute] Guid id)
        {
            bool commandResult = false;

            if (id != Guid.Empty)
            {
                var command = new DeleteRequestCommand(id);
                _logger.LogInformation("----- Sending command: {command})", nameof(command));
                commandResult = await _mediator.Send(command);
            }

            return commandResult ? Ok() : BadRequest();
        }
    }
}