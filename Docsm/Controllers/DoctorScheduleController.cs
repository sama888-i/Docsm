using Docsm.DTOs.DoctorScheduleDtos;
using Docsm.DTOs.SpecialtyDtos;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorScheduleController(ITimeScheduleService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSpecialties(int doctorId)
        {

            return Ok(await _service.GetAllSchedulesAsync(doctorId));
        }
        [HttpPost]
       
        public async Task<IActionResult> Create([FromBody]CreateScheduleDto  dto)
        {
            if (dto.startTime >= dto.endTime)
            {
                return BadRequest("Start time must be less than end time.");
            }

            if (!ModelState.IsValid) return BadRequest("Girilen melumatlarda yanlisliq var");
            await _service.CreateScheduleAsync(dto);
            return Ok();

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteScheduleAsync(id);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id,[FromBody]UpdateScheduleDto dto)
        {

            if (dto.startTime >= dto.endTime)
            {
                return BadRequest("Start time must be less than end time.");
            }
            if (!ModelState.IsValid) return BadRequest("Girilen melumatlarda yanlisliq var");
            await _service.UpdateScheduleAsync(id,dto);
            return Ok();
        }

    }
}
