using Tinder_Dating_API.Models.Constants;

namespace Tinder_Dating_API.Models.Core
{
    public class ApiExceptionResponse: ApiResponse
    {
        public string StackTrace { get; set; }
        public ApiExceptionResponse(string errorMessage=null, string stackTrace=null)
            :base(ErrorCodes.InternalServerError, errorMessage)
        {
            StackTrace = stackTrace;
        }
    }
}
