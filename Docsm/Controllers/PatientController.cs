using Docsm.DTOs.DoctorDtos;
using Docsm.DTOs.PatientDtos;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ProfileCreateOrUpdateDto dto)
        {

            if (!ModelState.IsValid) return BadRequest("Girilen melumatlarda yanlisliq var");
            await _service.ProfileCreateOrUpdateAsync(dto);
            return Ok();

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
