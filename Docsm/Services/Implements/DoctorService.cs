using AutoMapper;
using Docsm.DataAccess;
using Docsm.DTOs.DoctorDtos;
using Docsm.Exceptions;
using Docsm.Extensions;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Docsm.Exceptions.ImageException;

namespace Docsm.Services.Implements
{
    public class DoctorService(ApoSystemDbContext _context,IWebHostEnvironment _enw,
        IMapper _mapper,UserManager<User> _userManager ,IHttpContextAccessor _acc):IDoctorService
    {
        public async Task CreateAsync(DoctorCreateDto dto)
        {
            if (!await _context.Specialties.AnyAsync(x => x.Id == dto.SpecialtyId))
                throw new NotFoundException<Specialty>();
            var userId = _userManager.GetUserId(_acc.HttpContext.User);
            if (userId == null)
                throw new UnauthorizedAccessException<User>();

            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
                throw new NotFoundException("User not found with the given ID.");
            
            _mapper.Map(dto, existingUser);

            if (dto.Image != null)
            {
                if (!dto.Image.IsValidType("image"))
                    throw new InvalidImageTypeException("Image type must be an image");

                if (!dto.Image.IsValidSize(888))
                    throw new InvalidImageSizeException("Image length must be less than 888kb");

                existingUser.ProfileImageUrl = await dto.Image.UploadAsync(_enw.WebRootPath,"images","doctors");
            }

         
            var doctor = _mapper.Map<Doctor>(dto);
            doctor.UserId = existingUser.Id;
            await _context.Doctors.AddAsync(doctor);   
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            if (!id.HasValue) throw new BadRequestException("Girilen id sehvdir");
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

        public async Task UpdateAsync(int? id, DoctorUpdateDto dto)
        {
            if (id == null)
                throw new BadRequestException("Doctor id cannot be null.");

            if (!await _context.Specialties.AnyAsync(x => x.Id == dto.SpecialtyId))
                throw new NotFoundException<Specialty>();

            var doctor = await _context.Doctors
                .Include(d => d.User)  
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
                throw new NotFoundException<Doctor>();

            _mapper.Map(dto, doctor);
            
            if (dto.Image != null)
            {
                if (!dto.Image.IsValidType("image"))
                    throw new InvalidImageTypeException("Image type must be an image");

                if (!dto.Image.IsValidSize(888))
                    throw new InvalidImageSizeException("Image length must be less than 888kb");

                doctor.User.ProfileImageUrl = await dto.Image.UploadAsync(_enw.WebRootPath, "images", "doctors");
            }

            await _context.SaveChangesAsync();
        }

       
    }
}
