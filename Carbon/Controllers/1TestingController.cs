using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Carbon.Mediator.Infrastructure;
using Carbon.Data;
using Carbon.Data.Repositories.Interfaces;
using Carbon.API.Infrastructure.Models;
using Carbon.Model.Users;
using Carbon.Service.Interfaces;

namespace Carbon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class _1TestingController : BaseApiController
    {
        private readonly IApiClient _apiClient;
        private readonly CarbonDbContext _context;
        private readonly IUserDetailsService _userDetailsService;
        public _1TestingController(IApiClient apiClient, CarbonDbContext context, IUserDetailsService userDetailsService)
        {
            _apiClient = apiClient;
            _context = context;
            _userDetailsService = userDetailsService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseModel<UserDetails>> GetUserDetails(int id)
        {
            var userDetails = await _userDetailsService.GetByIdAsync(id);
            return Response(userDetails);
        }

        [HttpGet]
        [Route("db-context-testing")]
        public dynamic GetUserDetails()
        {
            var user = _context.UserDetails.Where(x => x.is_active == true).FirstOrDefault();
            return user;
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
