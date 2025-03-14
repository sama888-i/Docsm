using Docsm.DTOs.ReviewDtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Docsm.Services.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(ReviewCreateDto dto);
        Task DeleteReviewAsync(int reviewId);      
        Task<List<ReviewGetDto>> GetDoctorReviewsAsync(int doctorId);
        Task UpdateReviewAsync(int reviewId, ReviewUpdateDto dto);

    }
}
