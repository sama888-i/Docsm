using Docsm.DataAccess;
using Docsm.DTOs.DoctorDtos;
using Docsm.Helpers.Enums;
using Docsm.Helpers.Enums.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController(ApoSystemDbContext  _context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> SearchDoctor(int? specialtyId,Genders? gender)
        {
             var query = _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .Include(d=>d.Reviews)
            .Where(d => d.DoctorStatus == DoctorStatus.Approved)
            .AsQueryable();
            if (specialtyId.HasValue)
            {
                query = query.Where(d => d.SpecialtyId == specialtyId);
            }
            if (gender.HasValue)
            {
                query = query.Where(x => x.User.Gender == gender.Value);
            }
            var doctors = await query
           .Select(d => new SearchDoctorDto 
           {
               DoctorId  = d.Id,
               DoctorName = d.User.Name,
               DoctorImage =d.User.ProfileImageUrl,
               Specialty = d.Specialty.Name,
               ClinicAddress =d.ClinicAddress,
               Rating = d.Reviews.Any() ? (int)Math.Round(d.Reviews.Average(r => (double?)r.Rating) ?? 0) : 0,
               PricePerAppointment =d.PerAppointPrice 

           }).ToListAsync();
            return Ok(doctors);
        }
        [HttpGet("top-doctors")]
        public async Task<IActionResult> GetTopDoctors()
        {
            var topDoctors = await _context.Doctors
                .Include(x=>x.Specialty)
                .Where(d => d.Reviews.Any(r => r.Rating == 5)) 
                .Select(d => new
                {
                    d.Id,
                    d.User.Name,
                    d.User.Surname,
                    Specialty=d.Specialty.Name,
                    d.User.ProfileImageUrl,
                    Rating = d.Reviews.Where(r => r.Rating == 5).Average(r => r.Rating) ,
                    d.PerAppointPrice,
                    d.ClinicAddress 
                })
                .ToListAsync();

            return Ok(topDoctors);
        }

    }
}
