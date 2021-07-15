using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClients.Models
{
    public class ErrorModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public static ErrorModel New (int code, string message)
        {
            return new ErrorModel()
            {
                Code = code,
                Message = message
            };
        }
    }
}
