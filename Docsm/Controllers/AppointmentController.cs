using Docsm.DTOs.AppointmentDtos;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController(IAppointmentService _service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult>CreateAppointment(AppointmentCreateDto dto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("You must be logged in.");
            }
            await  _service.CreateAppointmentAsync(dto);
            return Ok();
        }
        [HttpPut("Confirm")]
        public async Task<IActionResult>ConfirmAppointment(int appointmentId)
        {
            await _service.ConfirmAppointmentAsync(appointmentId);
            return Ok();

        }
        [HttpDelete("Cancel")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            await _service.CancelAppointmentAsync(appointmentId);
            return Ok();
        }
    }
}
