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
            Status = 404;
            Message = "No Records";
        }

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
            Status = 200;
            Message = "Success";
        }

        public void SetSuccess(T data)
        {
            this.Data = data;
            Status = 200;
            Message = "Success";
        }

        public void SetFailure(string failureMessage)
        {
            Status = 417;
            Message = failureMessage;
        }

        public void SetAccessDenied()
        {
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
