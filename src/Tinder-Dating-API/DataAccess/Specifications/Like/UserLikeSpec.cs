using System;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.DataAccess.Specifications.Like
{
    public class UserLikeSpec: BaseSpecification<UserLike>
    {
        public UserLikeSpec(Guid sourcUserId, Guid likedUserId)
            :base(x => x.SourceUserId == sourcUserId 
                && x.LikedUserId == likedUserId)
        {
            
        }
    }
}
