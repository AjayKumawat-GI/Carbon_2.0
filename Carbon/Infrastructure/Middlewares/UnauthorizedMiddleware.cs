using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Referral.CoreApi.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Referral.API.Infrastructure.Middlewares
{
    public class UnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedMiddleware(
            RequestDelegate next
            )
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized && !context.Request.Headers.ContainsKey("Authorization"))
            {
                await UnauthorizedUser(context);
            }
        }

        private async Task UnauthorizedUser(HttpContext context)
        {
            var unauthorizedResponse = new ResponseModel<string>
            {
                Status = 401,
                Message = "Unauthorized User!",
                Data = "",
                Errors = null
            };
            context.Response.StatusCode = unauthorizedResponse.Status;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(unauthorizedResponse));
        }
    }
}
