using AutoMapper;
using Docsm.DTOs.DoctorDtos;
using Docsm.Models;

namespace Docsm.Profiles
{
    public class DoctorProfile:Profile 
    {
        public DoctorProfile()
        {
            CreateMap<DoctorCreateDto, User>();
                
            CreateMap<DoctorCreateDto, Doctor>();
                
            CreateMap<Doctor, DoctorGetDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.User.Name))
                .ForMember(d => d.Surname, opt => opt.MapFrom(s => s.User.Surname))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.User.PhoneNumber))
                .ForMember(d => d.Gender, opt => opt.MapFrom(s => s.User.Gender))
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(s => s.User.DateOfBirth))
                .ForMember(d => d.ProfileImageUrl, opt => opt.MapFrom(s => s.User.ProfileImageUrl));
        }

    
    }
}
