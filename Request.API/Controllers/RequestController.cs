using Microsoft.AspNetCore.Mvc;

namespace Request.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}