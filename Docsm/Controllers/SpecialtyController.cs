using Docsm.DataAccess;
using Docsm.DTOs.SpecialtyDtos;
using Docsm.Exceptions;
using Docsm.Extensions;
using Docsm.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController(ApoSystemDbContext _context,IWebHostEnvironment _enw) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSpecialties()
        {
            var specialty = await _context.Specialties
                .Select(s => new SpecialtyGetDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl
                }).ToListAsync();
            return Ok(specialty);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult>Create(SpecialtyCreateDto dto)
        {
            if (dto.Image != null)
            {
                if (!dto.Image.IsValidType("image"))
                    ModelState.AddModelError("Image", "Image type must be an image");
                if (!dto.Image.IsValidSize(888))
                    ModelState.AddModelError("Image", "Image length must be less than 888kb");
            }
            if (await _context.Specialties.AnyAsync(p => p.Name == dto.Name))
            {
                throw new ExistException<Specialty>();
            }
            if (!ModelState.IsValid) return BadRequest("Girilen melumatlarda yanlisliq var");
            var specialty = new Specialty
            { 
                Name = dto.Name,
                ImageUrl =await dto.Image!.UploadAsync(_enw.WebRootPath,"images","specialties")
            };
            await _context.Specialties.AddAsync(specialty);
            await _context.SaveChangesAsync();
            return Ok();

        }
        [HttpDelete]
        public async Task<IActionResult>Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var specialty = await _context.Specialties.FindAsync(id.Value);
            if (specialty is null)
                throw new NotFoundException<Specialty>();
            _context.Specialties.Remove(specialty);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult>Update(SpecialtyUpdateDto dto,int?id)
        {

            if (dto.Image != null)
            {
                if (!dto.Image.IsValidType("image"))
                    ModelState.AddModelError("Image", "Image type must be an image");
                if (!dto.Image.IsValidSize(888))
                    ModelState.AddModelError("Image", "Image length must be less than 888kb");
            }
            if (await _context.Specialties.AnyAsync(p => p.Name == dto.Name))
            {
                throw new ExistException<Specialty>();
            }
            if(!ModelState.IsValid)return BadRequest("Girilen melumatlarda yanlisliq var");
            if (!id.HasValue) return BadRequest();
            var specialty = await _context.Specialties.FindAsync(id.Value);
            if (specialty is null)
                throw new NotFoundException<Specialty>();
            specialty.Name = dto.Name;
            specialty.ImageUrl = await dto.Image!.UploadAsync(_enw.WebRootPath, "images", "specialties");
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
