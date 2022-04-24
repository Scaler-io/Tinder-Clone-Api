﻿using Microsoft.AspNetCore.Http;

namespace Tinder_Dating_API.Models.Requests.Photo
{
    public class UploadPhotoRequest
    {
        public IFormFile File { get; set; }      
    }

    public class DeletePhotoRequest
    {
        public string PublicId { get; set; }  
    }
}