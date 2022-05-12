using System;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.DataAccess.Specifications.User
{
    public class GetUserWithLikesSpec: BaseSpecification<AppUser>
    {
        public GetUserWithLikesSpec(Guid userId)
            :base(x => x.Id == userId)
        {
            AddIncludes("LikedUsers");
        }
    }
}
