using Docsm.DataAccess;
using Docsm.DTOs.SpecialtyDtos;
using Docsm.Exceptions;
using Docsm.Extensions;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        public async Task DeleteSpecialtyAsync(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty is null)
                throw new NotFoundException<Specialty>();
            var imagePath = Path.Combine(_enw.WebRootPath, specialty.ImageUrl);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
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

        public async Task<SpecialtyGetDto> UpdateSpecialtyAsync(SpecialtyUpdateDto dto, int id)
        {
           
            if (await _context.Specialties.AnyAsync(p => p.Name == dto.Name && p.Id != id))
            {
                throw new ExistException<Specialty>();
            }

            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty is null)
                throw new NotFoundException<Specialty>();

            if (dto.Image != null)
            {
                if (!dto.Image.IsValidType("image"))
                    throw new InvalidImageTypeException("Image type must be an image");

                if (!dto.Image.IsValidSize(888))
                    throw new InvalidImageSizeException("Image length must be less than 888kb");

                if (!string.IsNullOrEmpty(specialty.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_enw.WebRootPath, specialty.ImageUrl);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                specialty.ImageUrl = await dto.Image.UploadAsync(_enw.WebRootPath, "images", "specialties");
            }

            specialty.Name = dto.Name;

            await _context.SaveChangesAsync();
            return new SpecialtyGetDto
            {
                Id = specialty.Id,
                Name = specialty.Name,
                ImageUrl = specialty.ImageUrl
            };
        }

        
        public async Task<SpecialtyGetDto> GetByIdAsync(int id)
        {
            var specialty = await _context.Specialties.FirstOrDefaultAsync(x => x.Id == id);
            if (specialty == null)
                throw new NotFoundException<Specialty>();
            var specialtyResult = new SpecialtyGetDto
            {
                Id = specialty.Id,
                Name = specialty.Name,
                ImageUrl = specialty.ImageUrl 
            };

            return specialtyResult;

        }
       
    }
}
