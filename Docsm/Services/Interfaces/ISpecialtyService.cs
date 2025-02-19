using Docsm.DTOs.SpecialtyDtos;
using Docsm.Models;

namespace Docsm.Services.Interfaces
{
    public interface ISpecialtyService
    {
        Task<List<SpecialtyGetDto>> GetAllSpecialtiesAsync();
        Task CreateSpecialtyAsync(SpecialtyCreateDto dto);
        Task DeleteSpecialtyAsync(int id);
        Task UpdateSpecialtyAsync(SpecialtyUpdateDto dto, int id);
    }
}
