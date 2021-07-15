using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClients.Models
{
    public class CustomException
        : Exception
    {
        public readonly int Code;
        public readonly string Message;

        protected CustomException(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public static CustomException New(int code, string message)
        {
            return new CustomException(code, message);
        }
    }
}
