using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Carbon.Mediator.Infrastructure;

namespace Carbon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class _1TestingController : ControllerBase
    {
        private readonly IApiClient _apiClient;
        public _1TestingController(IApiClient apiClient)
        {
                _apiClient = apiClient;
        }

        [HttpPost]
        [Route("TestingAPIMediator")]
        public void Get()
        {
            _apiClient.CallApiAsync("https://webhook.site/c5f88d44-4c75-41cf-843f-0dbacb9c964d", HttpMethod.Post, "");
        }

        [HttpGet(Name = "ErrorTesting")]
        public void GetErrorTesting()
        {
            throw new NotImplementedException("Something went wrong");
        }

    }
}
