using Docsm.DTOs.DoctorDtos;
using Docsm.DTOs.PatientDtos;
using Docsm.Extensions;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Docsm.Exceptions.ImageException;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController(IPatientService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {

            return Ok(await _service.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            return Ok(patient);
        }
        [HttpPost("PatientProfile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateOrUpdate(ProfileCreateOrUpdateDto dto)
        {
            if (dto.Image != null)
            {
                if (!dto.Image.IsValidType("image"))
                    throw new InvalidImageTypeException("Image type must be an image");

                if (!dto.Image.IsValidSize(888))
                    throw new InvalidImageSizeException("Image length must be less than 888kb");    
            }
            await _service.ProfileCreateOrUpdateAsync(dto);
            return Ok();

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
