﻿using AutoMapper;
using Docsm.DTOs.PatientDtos;
using Docsm.Extensions;
using Docsm.Helpers.Enums;
using Docsm.Models;

namespace Docsm.Profiles
{
    public class PatientProfile:Profile 
    {
        public PatientProfile()
        {
            CreateMap<ProfileCreateOrUpdateDto ,User>()
                 .ForMember(p => p.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(p => p.Surname, opt => opt.MapFrom(src => src.Surname))
                 .ForMember(p => p.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                 .ForMember(p => p.Gender, opt => opt.MapFrom(src => src.Gender))
                 .ForMember(p => p.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<ProfileCreateOrUpdateDto, Patient>()
                 .ForMember(p => p.Address, opt => opt.MapFrom(src => src.Address))
                 .ForMember(p => p.Country, opt => opt.MapFrom(src => src.Country))
                 .ForMember(p => p.BloodGroup, opt => opt.MapFrom(src => (BloodGroups)src.BloodGroup));

            CreateMap<Patient,GetPatientProfileDto>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(p => p.Name, opt => opt.MapFrom(s => s.User.Name))
                .ForMember(p => p.Surname, opt => opt.MapFrom(s => s.User.Surname))
                .ForMember(p => p.PhoneNumber, opt => opt.MapFrom(s => s.User.PhoneNumber))
                .ForMember(p => p.Gender, opt => opt.MapFrom(s => s.User.Gender))
                .ForMember(p => p.DateOfBirth, opt => opt.MapFrom(s => s.User.DateOfBirth))
                .ForMember(p => p.ProfileImageUrl, opt => opt.MapFrom(s => s.User.ProfileImageUrl))
                .ForMember(p => p.BloodGroup, opt => opt.MapFrom(src => src.BloodGroup.ToString()));


        }
    }
}
