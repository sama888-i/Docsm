using Docsm.DTOs.DoctorDtos;

namespace Docsm.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorGetDto>> GetAllAsync();
        Task<DoctorGetDto> GetByIdAsync(int id);
        Task CreateOrUpdateAsync(DoctorCreateDto dto);
        Task DeleteAsync(int? id);
    }
}
