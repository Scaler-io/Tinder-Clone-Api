using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.DataAccess.Specifications.Identity
{
    public class FindUserByUserNameSpec : BaseSpecification<AppUser>
    {
        public FindUserByUserNameSpec(string username)
            :base(u => u.UserName.ToLower() == username.ToLower())
        {
            
        }
    }
}
