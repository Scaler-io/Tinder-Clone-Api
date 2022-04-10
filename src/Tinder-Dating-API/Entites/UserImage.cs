using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tinder_Dating_API.Entites
{
    [Table("Photos")]
    public class UserImage: BaseEntity
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        public Guid UserProfileId { get; set; }
        public UserProfile Profile { get; set; }
    }
}