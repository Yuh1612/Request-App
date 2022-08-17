using Microsoft.AspNetCore.Mvc;
using Request.API.Applications.Queries;
using System.Net;

namespace Request.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestQueries _requestQueries;
        public RequestController(IRequestQueries requestQueries)
        {
            _requestQueries = requestQueries;
        }

        [HttpGet]
        [Route("{userId}")]
        [HttpPut]
        [ProducesResponseType(typeof(List<LeaveRequestReponse>),(int)HttpStatusCode.OK)]
        public IActionResult GetLeaveRequests([FromRoute] Guid userId)
        {
            return Ok(_requestQueries.GetLeaveRequestByUserId(userId.ToString()).Result);
        }
    }
}