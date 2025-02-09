using Docsm.DataAccess;
using Docsm.DTOs.DoctorDtos;
using Docsm.DTOs.SpecialtyDtos;
using Docsm.Services.Implements;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController(IDoctorService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {

            return Ok(await _service.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _service.GetByIdAsync(id);
            return Ok(doctor);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(DoctorCreateDto dto)
        {

            if (!ModelState.IsValid) return BadRequest("Girilen melumatlarda yanlisliq var");
            await _service.CreateAsync(dto);
            return Ok();

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int? id, DoctorUpdateDto  dto)
        {

            if (!ModelState.IsValid) return BadRequest("Girilen melumatlarda yanlisliq var");
            await _service.UpdateAsync(id,dto);
            return Ok();
        }
    }
}
