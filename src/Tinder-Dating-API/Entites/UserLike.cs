using System;

namespace Tinder_Dating_API.Entites
{
    public class UserLike : BaseEntity
    {
        public AppUser SourceUser { get; set; }
        public Guid SourceUserId { get; set; }
        public AppUser LikedUser { get; set; }
        public Guid LikedUserId { get; set; }
    }
}
