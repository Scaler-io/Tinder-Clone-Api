using AutoMapper;
using System.Linq;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Mapper.User
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AppUser, MemberResponse>();
               

            CreateMap<UserProfile, MemberProfileResponse>()
                .ForMember(p => p.AddressLine, o => o.MapFrom(s => s.Address != null ? createAddressLine(s.Address) : null))
                .ForMember(p => p.Age, o => o.MapFrom(s => s.GetAge()))
                .ForMember(p => p.ImageUrl, o => o.MapFrom(s => s.Images.FirstOrDefault(i => i.IsMain == true).Url))
                .ForMember(p => p.Created, o => o.MapFrom(s => s.Created.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(p => p.LastActive, o => o.MapFrom(s => s.LastActive.ToString("yyyy-MM-dd hh:mm:ss tt")));
                

            CreateMap<UserImage, MemberImageResponse>();

            CreateMap<UserDetailsUpdateRequest, UserProfile>().ReverseMap();
        }

        private string createAddressLine(UserAddress address)
        {
            return $"Unit {address.UnitNumber}, {address.StreetNumber} {address.StreetName} {address.StreetType}, " +
                $"{address.City}, {address.State} - {address.PostCode}";
        }
    }
}
