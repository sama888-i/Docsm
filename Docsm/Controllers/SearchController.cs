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
               Rating = d.Reviews.Any() ? (int)Math.Round(d.Reviews.Average(r => (double?)r.Rating) ?? 0) : 0

           }).ToListAsync();
            return Ok(doctors);
        }
    }
}
