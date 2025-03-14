using Docsm.DataAccess;
using Docsm.DTOs.SpecialtyDtos;
using Docsm.Exceptions;
using Docsm.Extensions;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SpecialtyController(ISpecialtyService _service) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSpecialties()
        {

            return Ok(await _service.GetAllSpecialtiesAsync());
        }     
       
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(SpecialtyCreateDto dto)
        {
            await _service.CreateSpecialtyAsync(dto);
            return Ok(new { message = "İxtisas uğurla əlavə olundu!" });

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteSpecialtyAsync(id);
            return Ok();
        }
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult>Update([FromForm]SpecialtyUpdateDto dto,int id)
        {
            
             var updateSpecialty= await _service.UpdateSpecialtyAsync(dto,id);
            return Ok(updateSpecialty);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecialty(int id)
        {
            var specialty = await _service.GetByIdAsync(id);           
            return Ok(specialty);
        }
    }
}
