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
        private readonly ILogger<RequestController> _logger;

        public RequestController(IMediator mediator, ILogger<RequestController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPut]
        public async Task<ActionResult<string>> UpdateRequest([FromBody] UpdateRequestCommand updateRequestCommand)
        {
            await _mediator.Send(updateRequestCommand);
            return Ok();

       

        [HttpDelete]
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
    }
}