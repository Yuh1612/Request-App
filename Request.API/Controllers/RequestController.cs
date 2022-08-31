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
            return Ok(await _requestQueries.GetLeaveRequestByRequestorId());
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
            return Ok(await _requestQueries.GetLeaveRequestByApproverId());
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestCommand request)
        {
            bool requestResult = false;
            if (request != null && request?.ApproverId != Guid.Empty)
            {
                _logger.LogInformation("----- Sending reuqest: {reuqest})", nameof(request));
                requestResult = await _mediator.Send(request);
            }
            return requestResult ? Ok() : BadRequest();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRequest([FromBody] UpdateRequestCommand request)
        {
            bool requestResult = false;
            if (request != null && request.Id != Guid.Empty && request.DayOffStart != null && request.DayOffEnd != null)
            {
                _logger.LogInformation("----- Sending request: {request})", nameof(request));
                requestResult = await _mediator.Send(request);
            }
            return requestResult ? Ok() : BadRequest();
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRequest([FromRoute] DeleteRequestCommand request)
        {
            bool requestResult = false;
            if (request != null && request.Id != Guid.Empty)
            {
                _logger.LogInformation("----- Sending request: {request})", nameof(request));
                requestResult = await _mediator.Send(request);
            }
            return requestResult ? Ok() : BadRequest();
        }

        [HttpPost("approve")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AppproveRequest([FromBody] ApproveRequestCommand request)
        {
            bool requestResult = false;
            if (request.Id != Guid.Empty)
            {
                _logger.LogInformation("----- Sending command: {command})", nameof(request));
                requestResult = await _mediator.Send(request);
            }
            return requestResult ? Ok() : BadRequest();
        }

        [HttpPost("cancel")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelRequest([FromBody] CancelRequestCommand request)
        {
            bool requestResult = false;
            if (request.Id != Guid.Empty)
            {
                _logger.LogInformation("----- Sending command: {command})", nameof(request));
                requestResult = await _mediator.Send(request);
            }
            return requestResult ? Ok() : BadRequest();
        }
    }
}