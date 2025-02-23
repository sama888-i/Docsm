using Docsm.DataAccess;
using Docsm.Exceptions;
using Docsm.Helpers.Enums.Status;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(ApoSystemDbContext _context,IEmailService _service) : ControllerBase
    {
        [HttpPost("ApproveDoctor")]
        public async Task<IActionResult>ApproveDoctor(int doctorId)
        {
            var doctor=await _context.Doctors.Include(x=>x.User).FirstOrDefaultAsync(x=>x.Id==doctorId);
            if (doctor == null)
                throw new NotFoundException<Doctor>();
            if (doctor.DoctorStatus == DoctorStatus.Approved )
            {
                return BadRequest("Həkim artıq tesdiqlenib.");
            }
            doctor.DoctorStatus = DoctorStatus.Approved;
            await _context.SaveChangesAsync();
            var subject = "Hekim profiliniz tesdiqlendi";
            var body = "Artiq  Pasiyent qebul ede bilersiz";
            await _service.SendEmailAsync(doctor.User.Email , subject, body);
            return Ok();
        }
        [HttpPost("RejectDoctor")]
        public async Task<IActionResult>RejectDoctor(int doctorId)
        {
            var doctor = await _context.Doctors.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == doctorId);
            if (doctor == null)
                throw new NotFoundException<Doctor>();
            if (doctor.DoctorStatus == DoctorStatus.Rejected)
            {
                return BadRequest("Həkim artıq rədd edilib.");
            }
            doctor.DoctorStatus = DoctorStatus.Rejected;
            await _context.SaveChangesAsync();
            var subject = "Hekim profiliniz tesdiqlenmedi";
            var body = "Isteyiniz qebul edilmedi,contactdan elaqe saxlayin";
            await _service.SendEmailAsync(doctor.User.Email, subject, body);
            return Ok();
        }
        [HttpGet("GetDoctors")]
        public async Task<IActionResult> GetDoctors(DoctorStatus? status)
        {
            var query = _context.Doctors.Include(x => x.User).AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(x => x.DoctorStatus == status.Value);
            }

            var doctors = await query.Select(x => new
            {
                x.Id,
                x.User.Name,
                x.User.Surname,
                x.User.ProfileImageUrl,
                x.SpecialtyId,
                x.Adress ,
                x.ClinicName ,
                x.AboutMe,
                AverageRating = x.Reviews.Any(r => r.Rating.HasValue)
                ? x.Reviews.Where(r => r.Rating.HasValue).Average(r => r.Rating.Value)
                : 0,
                Status = x.DoctorStatus.ToString()  
            }).ToListAsync();

            return Ok(doctors);
        }
    }
}
