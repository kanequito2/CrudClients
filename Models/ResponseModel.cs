using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClients.Models
{
    public class ResponseModel<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public static ResponseModel<T> New(int code, string message, T data)
        {
            return new ResponseModel<T>()
            {
                StatusCode = code,
                Message = message,
                Data = data
            };
        }
    }
}
