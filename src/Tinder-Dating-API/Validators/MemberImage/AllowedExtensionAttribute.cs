using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Tinder_Dating_API.Validators.MemberImage
{
    public class AllowedExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, 
            ValidationContext validationContext)
        {
            var file = value as IFormFile;
        
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }   
            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : $"Only {JsonSerializer.Serialize(_extensions)} extensions are allowed";
        }
    }
}
