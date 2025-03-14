  using Docsm.DataAccess;
using Docsm.DTOs.ReviewDtos;
using Docsm.Exceptions;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Docsm.Services.Implements
{
    public class ReviewService(ApoSystemDbContext _context, IHttpContextAccessor _acc,UserManager<User> _userManager) :IReviewService
    {
        public async Task AddReviewAsync(ReviewCreateDto dto)
        {
            var userId = _userManager.GetUserId(_acc.HttpContext.User);
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
                throw new ExistException("You have already written a review for this doctor.");
           
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
        public async Task UpdateReviewAsync(int reviewId, ReviewUpdateDto dto)
        {
            var userId = _userManager.GetUserId(_acc.HttpContext.User);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException<User>();

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
                throw new NotFoundException<User>();

            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewId && r.PatientId == patient.Id);

            if (review == null)
                throw new NotFoundException<Review>();

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;

            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
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
