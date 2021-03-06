using System.Threading.Tasks;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.Services.Identity
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
