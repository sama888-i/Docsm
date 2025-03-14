using Docsm.DataAccess;
using Docsm.DTOs.AppointmentDtos;
using Docsm.DTOs.PatientDtos;
using Docsm.DTOs.PaymentDto;
using Docsm.DTOs.ReviewDtos;
using Docsm.Exceptions;
using Docsm.Helpers.Enums.Status;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Admin")]
    public class AdminController(ApoSystemDbContext _context,IEmailService _service) : ControllerBase
    {
        [HttpPost("ApproveDoctor")]
        public async Task<IActionResult>ApproveDoctor([FromQuery] int doctorId)
        {
            var doctor=await _context.Doctors.Include(x=>x.User).FirstOrDefaultAsync(x=>x.Id==doctorId);
            if (doctor == null)
                throw new NotFoundException<Doctor>();
            if (doctor.DoctorStatus == DoctorStatus.Approved )
            {
                return BadRequest("The doctor has already been approved.");
            }
            doctor.DoctorStatus = DoctorStatus.Approved;
            await _context.SaveChangesAsync();
            var subject = "Hekim profiliniz tesdiqlendi";
            var body = "Artiq  Pasiyent qebul ede bilersiz";
            await _service.SendEmailAsync(doctor.User.Email , subject, body);
            return Ok(new { message = "Doctor approved successfully" });
        }
        [HttpPost("RejectDoctor")]
        public async Task<IActionResult>RejectDoctor([FromQuery] int doctorId)
        {
            var doctor = await _context.Doctors.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == doctorId);
            if (doctor == null)
                throw new NotFoundException<Doctor>();
            if (doctor.DoctorStatus == DoctorStatus.Rejected)
            {
                return BadRequest("The doctor has already been rejected.");
            }
            doctor.DoctorStatus = DoctorStatus.Rejected;
            await _context.SaveChangesAsync();
            var subject = "Hekim profiliniz tesdiqlenmedi";
            var body = "Isteyiniz qebul edilmedi,contactdan elaqe saxlayin";
            await _service.SendEmailAsync(doctor.User.Email, subject, body);
            return Ok(new { message = "Doctor rejected successfully" });
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
                SpecialtyName=x.Specialty.Name,
                x.ClinicAddress  ,
                x.ClinicName ,
                x.AboutMe,
                AverageRating = x.Reviews.Any(r => r.Rating.HasValue)
                ? x.Reviews.Where(r => r.Rating.HasValue).Average(r => r.Rating.Value)
                : 0,
                Status = x.DoctorStatus.ToString()  
            }).ToListAsync();

            return Ok(doctors);
        }
        [HttpGet("GetReviews")]
        public async Task<List<AdminReviewDto>> GetAllReviewsForAdmin()
        {
            var reviews = await _context.Reviews

           .Include(r => r.Patient)
               .ThenInclude(r => r.User)
           .Include(r => r.Doctor)
               .ThenInclude(r => r.User)
          .Select(r => new AdminReviewDto
          {
              Id = r.Id,
              DoctorFullname = $"{r.Doctor.User.Name} {r.Doctor.User.Surname}",
              DoctorImage = r.Doctor.User.ProfileImageUrl,
              PatientFullname = $"{r.Patient.User.Name} {r.Patient.User.Surname}",
              PatientImage = r.Patient.User.ProfileImageUrl,
              Comment = r.Comment,
              Rating = r.Rating
          }).ToListAsync();
            return reviews;
        }
        [HttpGet("AllPatients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _context.Patients
                .Include(x => x.User)
                .Select(x => new GetAllPatientsForAdmin  
                { 
                    PatientId  =x.Id ,
                    PatientName =x.User.Name,
                    PatientImage =x.User.ProfileImageUrl,
                    PhoneNumber =x.PhoneNumber,
                    Email =x.User.Email 
                }).ToListAsync();
            return Ok(patients );
        }
        [HttpGet("Transaction")]
        [Authorize(Roles = "Admin,Patient,Doctor")]
        public async Task<IActionResult> GetAllTransaction()
        {
            var transaction = await _context.Payments
                .Include(x => x.Appointment)
                  .ThenInclude(x => x.Patient)
                .Include(x => x.Appointment.Doctor)                
                .Select(x => new TransactionDtoForAdmin
                { 
                    TransactionId = x.Id ,
                    PatientName=x.Appointment.Patient.User.Name,
                    PatientImage =x.Appointment.Patient.User.ProfileImageUrl,
                    DoctorName=x.Appointment.Doctor.User.Name,
                    DoctorImage=x.Appointment.Doctor.User.ProfileImageUrl,
                    Amount =x.Amount ,
                    Currency =x.Currency,
                    PaymentStatus =x.PaymentStatus.ToString(),
                    AppointmentDate=x.Appointment.DoctorTimeSchedule.AppointmentDate 


                }).ToListAsync();
                return Ok(transaction);
        }
        [HttpGet("Appointments")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                .Include (x=>x.Payment)
                .Include(x => x.Patient)
                   .ThenInclude(x=>x.User)
                .Include(x => x.Doctor)
                  .ThenInclude(x=>x.User)
                .Include(x=>x.DoctorTimeSchedule)
                .Select(x => new AppointmentGetDtoForAdmin
                {
                    DoctorName = x.Doctor.User.Name,
                    DoctorImage = x.Doctor.User.ProfileImageUrl,
                    PatientName = x.Patient.User.Name,
                    PatientImage = x.Patient.User.ProfileImageUrl,
                    AppointmentDate=x.DoctorTimeSchedule.AppointmentDate,
                    Amount = x.Payment != null ? x.Payment.Amount : 0m,
                    Status =x.Status

                }).ToListAsync();
            return Ok(appointments);
        }
    }
}
