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
            var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == doctorId);
            if (doctor == null)
                throw new NotFoundException<Doctor>();
            doctor.DoctorStatus = DoctorStatus.Rejected;
            await _context.SaveChangesAsync();
            var subject = "Hekim profiliniz tesdiqlenmedi";
            var body = "Isteyiniz qebul edilmedi,contactdan elaqe saxlayin";
            await _service.SendEmailAsync(doctor.User.Email, subject, body);
            return Ok();
        }
    }
}
