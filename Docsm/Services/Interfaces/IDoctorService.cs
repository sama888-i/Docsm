using Docsm.DTOs.DoctorDtos;

namespace Docsm.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorGetDto>> GetAllAsync();
        Task<DoctorGetDto> GetByIdAsync(int id);
        Task CreateAsync(DoctorCreateDto dto);
        Task UpdateAsync(int? id, DoctorUpdateDto dto);
        Task DeleteAsync(int? id);
    }
}
