using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Models.Requests;

namespace Tinder_Dating_API.DataAccess.Specifications.User
{
    public class UserWithFiltersCountSpec : BaseSpecification<AppUser>
    {
        public UserWithFiltersCountSpec(SpecParams param)
            : base(u =>
                (u.UserName != param.CurrentUserName) &&
                (param.GenderPreference == "all" || u.Profile.Gender == param.GenderPreference) &&
                (param.MaxAge == 0 || u.Profile.DateOfBirth.Year >= param.MinDob.Year) &&
                (param.MinAge == 0 || u.Profile.DateOfBirth.Year <= param.MaxDob.Year))   
        {

        }
    }
}
