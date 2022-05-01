using AutoMapper;
using System;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Models.Requests;

namespace Tinder_Dating_API.Mapper.User
{
    public class RegisterUserRequestMappingProfile: Profile
    {
        public RegisterUserRequestMappingProfile()
        {
            CreateMap<UserProfileRequest, UserProfile>()
                .ForPath(d => d.DateOfBirth, o => o.MapFrom(s => DateTime.Parse(s.DateOfBirth)))
                .ForPath(d => d.KnownAs, o => o.MapFrom(s => s.KnownAs))
                .ForPath(d => d.Gender, o => o.MapFrom(s => s.Gender))
                .ForPath(d => d.Address, o => o.MapFrom(s => s.Address));

            CreateMap<UserAddressRequest, UserAddress>();
        }   
    }
}
