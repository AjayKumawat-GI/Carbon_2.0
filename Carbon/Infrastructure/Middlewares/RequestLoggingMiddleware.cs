using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.API.Infrastructure.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                await LogRequestToDb(context);
                _logger.LogInformation(
                    "Request {method} {url} => {statusCode}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Response?.StatusCode,
                    context.Request?.Query,
                    context.Request?.QueryString.Value);
            }
        }

        private async Task LogRequestToDb(HttpContext context)
        {
            var reqBody = "";
            var request = context.Request;
            var response = context.Response;

            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                reqBody = await reader.ReadToEndAsync();
            }

            //var urlParameter = DataProvider.GetStringSqlParameter("@Url", request.Path.Value.ToString());
            //var requestMethodParameter = DataProvider.GetStringSqlParameter("@Method", request.Method.ToString());
            //var headersParameter = DataProvider.GetStringSqlParameter("@Headers", request.Headers.ToString());
            //var queryStringParameter = DataProvider.GetStringSqlParameter("@QueryString", request.QueryString.ToString());
            //var requestPayloadParameter = DataProvider.GetStringSqlParameter("@RequestPayload", reqBody);
            //var responseBodyParameter = DataProvider.GetStringSqlParameter("@Response", response.Body.ToString());

            //var sqlParams = new List<SqlParameter>
            //{
            //    urlParameter,
            //    requestMethodParameter,
            //    headersParameter,
            //    queryStringParameter,
            //    requestPayloadParameter,
            //    responseBodyParameter
            //};

            //await SqlHelper.ExecuteProcedureAsync<RequestLog>("USP_InsertRequestLogs", sqlParams);
        }
    }
}
