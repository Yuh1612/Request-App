using API.Exceptions;
using System.Net;

namespace Request.API.Applications.Commands
{
    public abstract class BaseCommandHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger _logger;

        protected BaseCommandHandler(IHttpContextAccessor httpContextAccessor, ILogger logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public Guid GetCurrentUserId()
        {
            if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "id").Value,
                    out var userId))
            {
                _logger.LogError("Cannot find user Id from token");
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return userId;
        }
    }
}