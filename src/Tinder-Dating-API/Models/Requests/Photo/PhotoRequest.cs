using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Tinder_Dating_API.Validators.MemberImage;

namespace Tinder_Dating_API.Models.Requests.Photo
{
    public class UploadPhotoRequest
    {
        [Required]
        [DataType(DataType.Upload)]
        [MaxPhotoSize(1)]
        [AllowedExtension(new string[] { ".jpg", ".png"})]
        public IFormFile File { get; set; }      
    }

    public class DeletePhotoRequest
    {
        public string PhotoId { get; set; }  
    }

    public class UpdatePhotoRequest
    {
        public string PhotoId { get; set; }
    }
}
