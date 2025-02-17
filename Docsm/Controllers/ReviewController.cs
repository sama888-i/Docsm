using Docsm.DTOs.ReviewDtos;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IReviewService _service) : ControllerBase
    {
       [HttpPost]
       public async Task<IActionResult>AddReview(ReviewCreateDto dto)
       {
            await _service.AddReviewAsync(dto);
            return Ok();
       }
       [HttpGet]
       public async Task<IActionResult>GetDoctorReview(int doctorId)
       {
          return Ok(await _service.GetDoctorReviewsAsync(doctorId));
       }
       [HttpGet("For Admin")]
       public async Task<IActionResult> GetAllReview()
       {
           
            return Ok(await _service.GetAllReviewsForAdminAsync());
       }
       [HttpDelete]
       public async Task<IActionResult> DeleteReview(int reviewId)
       {
            await _service.DeleteReviewAsync(reviewId);
            return Ok();
       }
       [HttpGet("For DoctorDashboard")]
        public async Task<IActionResult> GetDoctorDashboardReviews()
        {
           
            return Ok(await _service.GetDoctorDashboardReviewsAsync());
        }

    }
}
