using AutoMapper;
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
                throw new NotFoundException<User>();

           
            var existingPatient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (existingPatient != null)
            {
                _mapper.Map(dto, existingPatient);
                if (dto.Image != null)
                {
                  existingPatient.User.ProfileImageUrl = await dto.Image.UploadAsync(_enw.WebRootPath, "images", "patients");
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                _mapper.Map(dto, existingUser);
                if (dto.Image != null)
                {
                  existingUser.ProfileImageUrl = await dto.Image.UploadAsync(_enw.WebRootPath, "images", "patients");
                }
                var patient = _mapper.Map<Patient>(dto);
                patient.UserId = existingUser.Id;
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                throw new NotFoundException<Patient>();
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
        public async Task GetPatientAppointmentsAsync(int patientId)
        {
            var appointment = await _context.Appointments.Include(x=>x.PaymentIntentId)
                .Where(x => x.PatientId == patientId)
                .Select(x =>new
                {
                    x.Id,
                    x.Status,                   
                    Doctor = new 
                    {
                        x.Doctor.User.Name,
                        x.Doctor.User.Surname,
                        x.Doctor.User.ProfileImageUrl,
                        SpecialtyName= x.Doctor.Specialty.Name 
                    }
                }).ToListAsync();
            
        }
    }
}
