using Docsm.DataAccess;
using Docsm.DTOs.ReviewDtos;
using Docsm.Exceptions;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Docsm.Services.Implements
{
    public class ReviewService(ApoSystemDbContext _context, IHttpContextAccessor _acc) :IReviewService
    {
        public async Task AddReviewAsync(ReviewCreateDto dto)
        {
            string? userId = _acc.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException<User>();

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                       
            if (patient == null)
                throw new NotFoundException<User>();

            var doctorEntity = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == dto.DoctorId);
            if (doctorEntity == null)
                throw new NotFoundException<Doctor>();
           
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.DoctorId == dto.DoctorId && r.PatientId == patient.Id );
            if (existingReview != null )
                throw new ExistException("Bu həkim üçün artıq rəy yazmısınız.");
           
            var review = new Review
            {
                DoctorId = doctorEntity.Id ,
                PatientId = patient?.Id,
                Rating =dto.Rating ,
                Comment =dto.Comment ,
               

            };
            await _context.Reviews.AddAsync(review);   
            await  _context.SaveChangesAsync();

        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review= await _context.Reviews
               .FirstOrDefaultAsync(x=>x.Id==reviewId);
            if(review == null)
                throw new NotFoundException<Review>();
            
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

        }

        public async Task<List<AdminReviewDto>> GetAllReviewsForAdminAsync()
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

        public async Task<List<ReviewGetDto>> GetDoctorDashboardReviewsAsync()
        {
            string? userId = _acc.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException<User>();

            var doctor = await _context.Doctors.FirstOrDefaultAsync(p => p.UserId == userId);
            if (doctor == null)
                throw new NotFoundException<Doctor>();

            var reviews = await _context.Reviews
                .Where(r => r.DoctorId == doctor.Id) 
                .Include(r => r.Patient)
                    .ThenInclude(r=>r.User)
                .Select(r => new ReviewGetDto
                {
                    Id = r.Id,
                    PatientFullname = $"{r.Patient.User.Name} {r.Patient.User.Surname}",
                    PatientImage = r.Patient.User.ProfileImageUrl,
                    Comment = r.Comment,
                    Rating = r.Rating
                }).ToListAsync();
            return reviews;
        }

        public async Task<List<ReviewGetDto>> GetDoctorReviewsAsync(int doctorId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.DoctorId == doctorId)
                .Include(r => r.Patient)
                   .ThenInclude(r=>r.User)                
               
                .Select(r => new ReviewGetDto
                {
                    Id = r.Id,
                    PatientFullname = $"{r.Patient.User.Name } {r.Patient.User.Surname}",
                    PatientImage = r.Patient.User.ProfileImageUrl,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    
                }).ToListAsync();
            return reviews; 
        }
    }
}
