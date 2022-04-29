using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tinder_Dating_API.Validators.MemberImage
{
    public class MaxPhotoSizeAttribute : ValidationAttribute
    {
        private readonly int _maxPhotoSize;

        public MaxPhotoSizeAttribute(int maxPhotoSize)
        {
            _maxPhotoSize = maxPhotoSize;
        }

        protected override ValidationResult IsValid(object value, 
            ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if(file != null)
            {
                if(file.Length > _maxPhotoSize * 1024 * 1024)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : $"Maximum allowed file size is {_maxPhotoSize} MB.";
        }
    }
}
