using Docsm.DTOs.ReviewDtos;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
      
       [HttpDelete("{reviewId}")]
       public async Task<IActionResult> DeleteReview( int reviewId)
       {
            await _service.DeleteReviewAsync(reviewId);
            return Ok();
       }
       [HttpPut]
       public async Task<IActionResult>UpdateReview(int reviewId, ReviewUpdateDto dto)
        {
            await _service.UpdateReviewAsync(reviewId, dto);
            return Ok();
        }
      

    }
}
