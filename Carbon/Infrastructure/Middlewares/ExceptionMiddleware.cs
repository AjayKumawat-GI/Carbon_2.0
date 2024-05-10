using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Carbon.API.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Carbon.Service.Interfaces;
using System.Diagnostics;

namespace Carbon.API.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly IExceptionLogging _exceptionLogging;

        public ExceptionMiddleware(
            RequestDelegate next
            , ILogger<ExceptionMiddleware> logger
            , IHttpContextAccessor accessor
            , IExceptionLogging exceptionLogging
            )
        {
            _next = next;
            _logger = logger;
            _accessor = accessor;
            _exceptionLogging = exceptionLogging;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogExceptionToLogFile(context, ex);
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

        private async Task LogExceptionToLogFile(HttpContext context, Exception ex)
        {
            // Get the stack trace from the exception
            var stackTrace = new StackTrace(ex);

            // Get the top-most stack frame (the one that threw the exception)
            var stackFrame = stackTrace.GetFrame(0);

            if (stackFrame != null)
            {
                // Get the method that threw the exception
                var method = stackFrame.GetMethod();

                if (method != null)
                {
                    // Extract the class name and method name
                    var className = method.ReflectedType?.FullName ?? "UnknownClass";
                    var methodName = method.Name;

                    var errorDescription = ex.InnerException != null ? ex.InnerException.Message : ex.StackTrace;
                    // Log the exception with the class name and method name
                    //_logger.LogError(ex, $"An error occurred in class: {className}, method: {methodName}");
                    _exceptionLogging.blnLogError("", className, methodName, ex.Message, errorDescription);
                    return;
                }
            }

            // Log the exception without class and method name if stack frame or method info is unavailable
            _logger.LogError(ex, "An error occurred.");
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
            response.Success = false;
            response.Message = ex.Message.ToString();
            return response;
        }
    }
}
