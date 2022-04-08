using System.Collections.Generic;
using Tinder_Dating_API.Models.Constants;

namespace Tinder_Dating_API.Models.Core
{
    public class ApiValidationResponse : ApiResponse
    {
        public List<FieldLevelError> Errors { get; set; }
        public ApiValidationResponse()
            :base(ErrorCodes.UnprocessableEntity)
        {
            Message = GetDefaultMessgae(ErrorCodes.UnprocessableEntity);
        }

        protected override string GetDefaultMessgae(string statusCode)
        {
            return statusCode switch
            {
                ErrorCodes.UnprocessableEntity => "Inputs are invalid",
                _ => null
            };
        }
    }

    public class FieldLevelError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Field { get; set; }
    }
}
