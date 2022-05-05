using System;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Models.Requests;

namespace Tinder_Dating_API.DataAccess.Specifications.User
{
    public class GetUserWithProfileInfoSpec: BaseSpecification<AppUser>
    {
        public GetUserWithProfileInfoSpec(SpecParams param)
            : base(u =>
                  (u.UserName != param.CurrentUserName) &&
                  (param.GenderPreference == "all" || u.Profile.Gender == param.GenderPreference) &&
                  (param.MaxAge == 0 || u.Profile.DateOfBirth.Year >= param.MinDob.Year)&&
                  (param.MinAge == 0 || u.Profile.DateOfBirth.Year <= param.MaxDob.Year))
        {
            AddIncludes("Profile.Address");
            AddIncludes("Profile.Images");
            ApplyPaging(param.PageSize * (param.PageIndex - 1), param.PageSize);
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort)
                {
                    case SortParams.CreatedAsc:
                        AddOrderBy(u => u.Profile.Created);
                        break;
                    case SortParams.CreatedDesc:
                        AddOrderByDescending(u => u.Profile.Created);
                        break;
                    case SortParams.LastActiveAsc:
                        AddOrderBy(u => u.Profile.LastActive);
                        break;
                    case SortParams.LastActiveDesc:
                        AddOrderByDescending(u => u.Profile.LastActive);
                        break;
                    default:
                        break;
                }
            }
        }
        public GetUserWithProfileInfoSpec(Guid id)
            :base(u => u.Id == id)
        {
            AddIncludes("Profile.Address");
            AddIncludes("Profile.Images");
            AddIncludes("UserRoles");
        }
    }
}
