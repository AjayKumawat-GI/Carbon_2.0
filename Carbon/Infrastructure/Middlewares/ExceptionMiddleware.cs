using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Carbon.API.Infrastructure.Models;
//using Carbon.Logging;
//using Carbon.Logging.Interfaces;
//using Carbon.Model.Exception;
//using Carbon.Utility.DataHelper;
//using Carbon.Utility.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Carbon.API.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHttpContextAccessor _accessor;

        public ExceptionMiddleware(
            RequestDelegate next
            , ILogger<ExceptionMiddleware> logger
            , IHttpContextAccessor accessor
            )
        {
            _next = next;
            _logger = logger;
            _accessor = accessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogExceptionToDb(context, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var exceptionData = GetExceptionDetails(ex);
            context.Response.StatusCode = (int)exceptionData.Status;
            await context.Response.WriteAsync(exceptionData.ToString());
        }

        private ResponseModel<string> GetExceptionDetails(Exception ex)
        {
            var model = GetErrorResponseAsync(ex);

            var exceptionType = ex.GetType();
            var value = GetValueByExceptionType(exceptionType);

            model.Status = value switch
            {
                1 => (int)HttpStatusCode.Conflict,
                2 or 5 => (int)HttpStatusCode.NotFound,
                3 => (int)HttpStatusCode.BadRequest,
                4 or 8 => (int)HttpStatusCode.UnprocessableEntity,
                6 => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError,
            };
            return model;
        }

        private int GetValueByExceptionType(Type exceptionType)
        {
            var errorCode = new Dictionary<Type, int>
            {
                {typeof(ValidationException), 4},
                {typeof(NullReferenceException), 5},
                {typeof(SecurityTokenExpiredException), 6},
            };

            return errorCode.Where(x => x.Key == exceptionType)
                            .Select(x => x.Value)
                            .FirstOrDefault();
        }

        private async Task LogExceptionToDb(HttpContext context, Exception ex)
        {
            //var exMesssageParameter = DataProvider.GetStringSqlParameter("@Message", ex.Message.ToString());
            //var exTypeParameter = DataProvider.GetStringSqlParameter("@Type", ex.GetType().ToString());
            //var exSourceParameter = DataProvider.GetStringSqlParameter("@Source", ex.StackTrace.ToString());
            //var urlParameter = DataProvider.GetStringSqlParameter("@Url", context.Request?.Path.Value.ToString());

            //var sqlParams = new List<SqlParameter>
            //{
            //    exMesssageParameter,
            //    exTypeParameter,
            //    exSourceParameter,
            //    urlParameter
            //};

            //await SqlHelper.ExecuteProcedureAsync<ReferralExceptionLog>("USP_InsertException", sqlParams);
        }

        private ResponseModel<string> GetErrorResponseAsync(Exception ex)
        {
            var response = new ResponseModel<string>();

            string[] errorMessages = new string[1];
            errorMessages[0] = ex.StackTrace.ToString();
            //var errorList = new List<Errors>
            //{
            //    new Errors
            //    {
            //        PropertyName = ex.GetType().ToString(),
            //        ErrorMessages = errorMessages
            //    }
            //};

            //response.Errors = errorList;
            response.Message = ex.Message.ToString();
            return response;
        }
    }
}
