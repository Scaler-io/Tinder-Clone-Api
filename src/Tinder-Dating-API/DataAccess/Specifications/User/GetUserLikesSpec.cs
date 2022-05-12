using System;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.DataAccess.Specifications.User
{
    public class GetUserLikesSpec: BaseSpecification<AppUser>
    {
        public GetUserLikesSpec(string predicate, Guid userId)
            :base()
        {
            AddOrderBy(u => u.UserName);
        }
    }
}
