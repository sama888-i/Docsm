using Docsm.DataAccess;
using Docsm.DTOs.SpecialtyDtos;
using Docsm.Exceptions;
using Docsm.Extensions;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Docsm.Exceptions.ImageException;

namespace Docsm.Services.Implements
{
    public class SpecialtyService(ApoSystemDbContext _context,IWebHostEnvironment _enw) : ISpecialtyService
    {
        public async Task CreateSpecialtyAsync(SpecialtyCreateDto dto)
        {

            if (dto.Image != null)
            {
                if (!dto.Image.IsValidType("image"))
                    throw new InvalidImageTypeException("Image type must be an image");

                if (!dto.Image.IsValidSize(888))
                    throw new InvalidImageSizeException("Image length must be less than 888kb");
            }
        
            if (await _context.Specialties.AnyAsync(p => p.Name == dto.Name))
            {
                throw new ExistException<Specialty>();
            }           
            var specialty = new Specialty
            {
                Name = dto.Name,
                ImageUrl = await dto.Image!.UploadAsync(_enw.WebRootPath, "images", "specialties")
            };
            await _context.Specialties.AddAsync(specialty);
            await _context.SaveChangesAsync();
          
        }

        public async Task DeleteSpecialtyAsync(int? id)
        {
            if (!id.HasValue) throw new BadRequestException("Girilen id sehvdir");
            var specialty = await _context.Specialties.FindAsync(id.Value);
            if (specialty is null)
                throw new NotFoundException<Specialty>();
            _context.Specialties.Remove(specialty);
            await _context.SaveChangesAsync();
            
        }

        public async Task<List<SpecialtyGetDto>> GetAllSpecialtiesAsync()
        {
            var specialty = await _context.Specialties
                .Select(s => new SpecialtyGetDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl
                }).ToListAsync();
            return (specialty);
        }

        public async Task UpdateSpecialtyAsync(SpecialtyUpdateDto dto, int? id)
        {

            if (dto.Image != null)
            {
                if (!dto.Image.IsValidType("image"))
                    throw new InvalidImageTypeException("Image type must be an image");

                if (!dto.Image.IsValidSize(888))
                    throw new InvalidImageSizeException("Image length must be less than 888kb");
            }
        
            if (await _context.Specialties.AnyAsync(p => p.Name == dto.Name))
            {
                throw new ExistException<Specialty>();
            }
           
            if (!id.HasValue) throw new BadRequestException("Girilen id sehvdir");
            var specialty = await _context.Specialties.FindAsync(id.Value);
            if (specialty is null)
                throw new NotFoundException<Specialty>();
            specialty.Name = dto.Name;
            specialty.ImageUrl = await dto.Image!.UploadAsync(_enw.WebRootPath, "images", "specialties");
            await _context.SaveChangesAsync();
            
        }
    }
}
