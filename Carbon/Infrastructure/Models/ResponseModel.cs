using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Carbon.API.Infrastructure.Models
{
    public class ResponseModel<T>
    {
        public ResponseModel()
        {
            ApiErrorCode = 0;
            Success = true;
            Status = 404;
            Message = "No Records";
        }

        public int ApiErrorCode { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<Errors> Errors { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public void SetSuccess()
        {
            ApiErrorCode = 0;
            Success = true;
            Status = 200;
            Message = "Success";
        }

        public void SetSuccess(T data)
        {
            this.Data = data;
            ApiErrorCode = 0;
            Success = true;
            Status = 200;
            Message = "Success";
        }

        public void SetFailure(string failureMessage)
        {
            ApiErrorCode = 0;
            Success = false;
            Status = 417;
            Message = failureMessage;
        }

        public void SetAccessDenied()
        {
            ApiErrorCode = 0;
            Success = false;
            Status = 703;
            Message = "Access Denied";
        }

        public void SetStatus(bool isSuccess)
        {
            if (isSuccess)
            {
                SetSuccess();
            }
            else
                SetFailure("");
        }

        public void SetInvalidModel()
        {
            ApiErrorCode = 0;
            Success = false;
            Status = 422;
            Message = "Invalid Model";
        }


        public void AddError(Errors error)
        {
            if (Errors == null)
            {
                Errors = new List<Errors>();
            }
            Errors.Add(error);
        }

        public bool IsSuccess()
        {
            return Status == 200 ? true : false;
        }
    }

    public class Errors
    {
        public string PropertyName { get; set; }
        public string[] ErrorMessages { get; set; }
    }
}
