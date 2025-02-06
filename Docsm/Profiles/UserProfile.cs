using AutoMapper;
using Docsm.DTOs.AuthDtos;
using Docsm.Models;

namespace Docsm.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToDateTime(new TimeOnly(0, 0))));
        }
    }
}
