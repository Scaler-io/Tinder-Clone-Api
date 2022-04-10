using Microsoft.EntityFrameworkCore;
using System;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.DataAccess.Specifications.User
{
    public class GetUserWithProfileInfoSpec: BaseSpecification<AppUser>
    {
        public GetUserWithProfileInfoSpec()
        {
            AddIncludes("Profile.Address");
            AddIncludes("Profile.Images");
        }
        public GetUserWithProfileInfoSpec(Guid id)
            :base(u => u.Id == id)
        {
            AddIncludes("Profile.Address");
            AddIncludes("Profile.Images");
        }
    }
}
