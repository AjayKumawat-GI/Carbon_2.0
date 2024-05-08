using Microsoft.AspNetCore.Builder;
using Referral.API.Infrastructure.Middlewares;
using Referral.CoreApi.Infrastructure.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Referral.API.Infrastructure.Extensions
{
    public static class ErrorLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
