using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public class Result<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T Data { get; }

        private Result(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static Result<T> Ok(T data, string message = "")
        {
            return new Result<T>(true, message, data);
        }

        public static Result<T> Fail(string message)
        {
            return new Result<T>(false, message, default(T));
        }
    }
}
