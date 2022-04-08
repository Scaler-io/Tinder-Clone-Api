using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tinder_Dating_API.Models.Core
{
    public class Result<T>
    {
        public bool IsSucess { get; set; }
        public T Value { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public static Result<T> Success(T value)
        {
            return new Result<T> { IsSucess = true, Value = value };
        }

        public static Result<T> Fail(string errorCode, string erroMessgae)
        {
            return new Result<T> { IsSucess = false, ErrorCode = errorCode, ErrorMessage = erroMessgae };
        }
    }
}
