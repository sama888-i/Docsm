using Docsm.DTOs.AppointmentDtos;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AppointmentController(IAppointmentService _service) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult>CreateAppointment(AppointmentCreateDto dto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("You must be logged in.");
            }
            var result=await  _service.CreateAppointmentAsync(dto);
            return Ok(result);
        }
        [HttpPut("Confirm")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult>ConfirmAppointment(int appointmentId)
        {
             var result= await _service.ConfirmAppointmentAsync(appointmentId);
            return Ok(result);

        }
        [HttpPut("Cancel")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var result=await _service.CancelAppointmentAsync(appointmentId);
            return Ok(result );
        }
        [HttpPut("Complete")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult>CompleteAppointment(int appointmentId)
        {
            var result=await _service.CompleteAppointmentAsync(appointmentId);
            return Ok(result);
        }
        
    }
}
