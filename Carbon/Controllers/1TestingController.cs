using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carbon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class _1TestingController : ControllerBase
    {
        [HttpGet(Name = "ErrorTesting")]
        public void GetErrorTesting()
        {
            throw new NotImplementedException("Something went wrong");
        }
    }
}
