using Economy.Persistence.Services;
using Microsoft.AspNetCore.Mvc;

namespace Economy.AuthServer.API.Controllers
{
    //[Route("api/[controller]/[action]")]

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
	{
	

        [NonAction]
        public ObjectResult CreateResult<T>(ServiceResult<T> result)
        {
            return new ObjectResult(result)
            {
                StatusCode = (int)result.Status
            };
        }

        [NonAction]
        public ObjectResult CreateResult(ServiceResult result)
        {
            return new ObjectResult(result)
            {
                StatusCode = (int)result.Status
            };
        }

    }
}
