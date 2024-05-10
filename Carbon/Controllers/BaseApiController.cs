using Carbon.API.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carbon.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        [NonAction]
        public ResponseModel<T> Response<T>(T data)
        {
            var result = new ResponseModel<T>();

            if (data != null)
            {
                result.SetSuccess(data);
            }

            return result;
        }

        [NonAction]
        public ResponseModel<List<T>> Response<T>(List<T> data)
        {
            var result = new ResponseModel<List<T>>();

            if (data != null)
            {
                if (data.Count > 0)
                    result.SetSuccess(data);
                else
                    result.Data = data;
            }

            return result;
        }
    }
}
