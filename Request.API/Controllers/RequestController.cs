using MediatR;
using Microsoft.AspNetCore.Mvc;
using Request.API.Applications.Commands;

namespace Request.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateRequest([FromBody] UpdateRequestCommand updateRequestCommand)
        {
            await _mediator.Send(updateRequestCommand);
            return Ok();
        }
    }
}