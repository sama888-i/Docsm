﻿using AutoMapper;
using Docsm.DataAccess;
using Docsm.DTOs.DoctorDtos;
using Docsm.DTOs.PatientDtos;
using Docsm.Exceptions;
using Docsm.Extensions;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Docsm.Exceptions.ImageException;

namespace Docsm.Services.Implements
{
    public class PatientService(
        ApoSystemDbContext _context,
        IMapper _mapper,
        UserManager<User> _userManager, 
        IHttpContextAccessor _acc,
        IWebHostEnvironment _enw) : IPatientService
    {
        public async Task<List<GetPatientProfileDto>> GetAllAsync()
        {
            var patients = await _context.Patients
            .Include(p => p.User)
            .ToListAsync();

            return _mapper.Map<List<GetPatientProfileDto>>(patients);
        }

        public async Task<GetPatientProfileDto> GetByIdAsync(int id)
        {
             var patient = await _context.Patients
            .Include(p => p.User) 
            .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                throw new NotFoundException<Patient>();

            return _mapper.Map<GetPatientProfileDto>(patient);
        }

        public async Task ProfileCreateOrUpdateAsync(ProfileCreateOrUpdateDto dto)
        {
            var userId = _userManager.GetUserId(_acc.HttpContext.User);
            if (userId == null)
                throw new UnauthorizedAccessException<User>();

            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
                throw new NotFoundException("User not found with the given ID.");

           
            var existingPatient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (existingPatient != null)
            {
                _mapper.Map(dto, existingPatient);
                if (dto.Image != null)
                {
                    if (!dto.Image.IsValidType("image"))
                        throw new InvalidImageTypeException("Image type must be an image");

                    if (!dto.Image.IsValidSize(888))
                        throw new InvalidImageSizeException("Image length must be less than 888kb");

                    existingPatient.User.ProfileImageUrl = await dto.Image.UploadAsync(_enw.WebRootPath, "images", "patients");
                }

                await _context.SaveChangesAsync();
            }
            else
            {

                _mapper.Map(dto, existingUser);
                if (dto.Image != null)
                {
                    if (!dto.Image.IsValidType("image"))
                        throw new InvalidImageTypeException("Image type must be an image");

                    if (!dto.Image.IsValidSize(888))
                        throw new InvalidImageSizeException("Image length must be less than 888kb");

                    existingUser.ProfileImageUrl = await dto.Image.UploadAsync(_enw.WebRootPath, "images", "patients");
                }

                var patient = _mapper.Map<Patient>(dto);
                patient.UserId = existingUser.Id;
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(int? id)
        {
            if (!id.HasValue) throw new BadRequestException("Girilen id sehvdir");
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                throw new NotFoundException<Patient>();
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}
