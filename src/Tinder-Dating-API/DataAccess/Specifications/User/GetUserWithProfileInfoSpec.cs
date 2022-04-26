using System;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Models.Requests;

namespace Tinder_Dating_API.DataAccess.Specifications.User
{
    public class GetUserWithProfileInfoSpec: BaseSpecification<AppUser>
    {
        public GetUserWithProfileInfoSpec(SpecParams param)
        {
            AddIncludes("Profile.Address");
            AddIncludes("Profile.Images");
            ApplyPaging(param.PageSize * (param.PageIndex - 1), param.PageSize);
        }
        public GetUserWithProfileInfoSpec(Guid id)
            :base(u => u.Id == id)
        {
            AddIncludes("Profile.Address");
            AddIncludes("Profile.Images");
        }
    }
}
