using Docsm.DTOs.DoctorDtos;
using Docsm.DTOs.PatientDtos;

namespace Docsm.Services.Interfaces
{
    public interface IPatientService
    {
        Task<List<GetPatientProfileDto>> GetAllAsync();
        Task<GetPatientProfileDto> GetByIdAsync(int id);
        Task ProfileCreateOrUpdateAsync(ProfileCreateOrUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
