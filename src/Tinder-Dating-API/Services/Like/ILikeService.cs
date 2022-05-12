using System.Threading.Tasks;
using Tinder_Dating_API.Models.Core;

namespace Tinder_Dating_API.Services.Like
{
    public interface ILikeService
    {
        Task<Result<bool>> AddLike(string username);
    }
}
