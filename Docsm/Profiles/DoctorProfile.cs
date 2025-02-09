using AutoMapper;
using Docsm.DTOs.DoctorDtos;
using Docsm.Models;

namespace Docsm.Profiles
{
    public class DoctorProfile:Profile 
    {
        public DoctorProfile()
        {
            CreateMap<DoctorCreateDto, User>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(d => d.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<DoctorCreateDto, Doctor>()
                .ForMember(d => d.SpecialtyId, opt => opt.MapFrom(src => src.SpecialtyId))
                .ForMember(d => d.Adress , opt => opt.MapFrom(src => src.Adress))
                .ForMember(d => d.AboutMe, opt => opt.MapFrom(src => src.AboutMe))
                .ForMember(d => d.Services, opt => opt.MapFrom(src => src.Services))
                .ForMember(d => d.ClinicName, opt => opt.MapFrom(src => src.ClinicName));


            CreateMap<DoctorUpdateDto, User>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(d => d.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

          
            CreateMap<DoctorUpdateDto, Doctor>()
                .ForMember(d => d.SpecialtyId, opt => opt.MapFrom(src => src.SpecialtyId))
                .ForMember(d => d.Adress, opt => opt.MapFrom(src => src.Adress))
                .ForMember(d => d.AboutMe, opt => opt.MapFrom(src => src.AboutMe))
                .ForMember(d => d.Services, opt => opt.MapFrom(src => src.Services))
                .ForMember(d => d.ClinicName, opt => opt.MapFrom(src => src.ClinicName));


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
