﻿using Docsm.Helpers.Enums;

namespace Docsm.DTOs.PatientDtos
{
    public class GetPatientProfileDto
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public Genders Gender { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public BloodGroups BloodGroup { get; set; }
        public string? ProfileImageUrl{  get; set; }
    }
}
