using AutoMapper;
using Docsm.DataAccess;
using Docsm.DTOs.DoctorDtos;
using Docsm.Exceptions;
using Docsm.Extensions;
using Docsm.Helpers.Enums.Status;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;
using static Docsm.Exceptions.ImageException;

namespace Docsm.Services.Implements
{
    public class DoctorService(ApoSystemDbContext _context,IWebHostEnvironment _enw,
        IMapper _mapper,UserManager<User> _userManager ,IHttpContextAccessor _acc,
        IEmailService _service):IDoctorService
    {
        [Authorize]
        public async Task CreateOrUpdateAsync(DoctorCreateDto dto)
        {
            if (!await _context.Specialties.AnyAsync(x => x.Id == dto.SpecialtyId))
                throw new NotFoundException<Specialty>();

            string? userId = _acc.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException<User>();

            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
                throw new NotFoundException<User>();

            var existingDoctor = await _context.Doctors .FirstOrDefaultAsync(d => d.UserId == userId);

            if (existingDoctor != null)
            {
           
                _mapper.Map(dto, existingDoctor);

                if (dto.Image != null)
                {
                    if (!dto.Image.IsValidType("image"))
                        throw new InvalidImageTypeException("Image type must be an image");

                    if (!dto.Image.IsValidSize(888))
                        throw new InvalidImageSizeException("Image length must be less than 888kb");

                    existingDoctor.User.ProfileImageUrl = await dto.Image.UploadAsync(_enw.WebRootPath, "images", "doctors");
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

                    existingUser.ProfileImageUrl = await dto.Image.UploadAsync(_enw.WebRootPath, "images", "doctors");
                }

                var doctor = _mapper.Map<Doctor>(dto);
                doctor.UserId = existingUser.Id;
                doctor.DoctorStatus = DoctorStatus.Pending;
                var subject = "Doccure doctor profile";
                var body = "Profiliniz yaradilib ,Adminin tesdiqini gozleyin";
                await _service.SendEmailAsync(doctor.User.Email!, subject, body);
                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {            
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                throw new NotFoundException<Doctor>();
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DoctorGetDto>> GetAllAsync()
        {
            var doctors = await _context.Doctors
                  .Include(x => x.User)
                  .Include(x => x.Specialty).ToListAsync();
            return _mapper.Map<List<DoctorGetDto>>(doctors);
        }

        public async Task<DoctorGetDto> GetByIdAsync(int id)
        {
            var doctor = await _context.Doctors
                  .Include(x => x.User)
                  .Include(x => x.Specialty).FirstOrDefaultAsync(x=>x.Id == id);
            if (doctor == null)
                throw new NotFoundException<Doctor>();
            return _mapper.Map<DoctorGetDto>(doctor);
        }

       

       
    }
}
