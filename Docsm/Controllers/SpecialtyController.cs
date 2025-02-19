using Docsm.DataAccess;
using Docsm.DTOs.SpecialtyDtos;
using Docsm.Exceptions;
using Docsm.Extensions;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController(ISpecialtyService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSpecialties()
        {
           
            return Ok(await _service.GetAllSpecialtiesAsync());
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult>Create(SpecialtyCreateDto dto)
        {           
            await _service.CreateSpecialtyAsync(dto);
            return Ok();

        }
        [HttpDelete]
        public async Task<IActionResult>Delete(int id)
        {
            await _service.DeleteSpecialtyAsync(id);
            return Ok();
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult>Update(SpecialtyUpdateDto dto,int id)
        {
            await _service.UpdateSpecialtyAsync(dto,id);
            return Ok();
        }
    }
}
