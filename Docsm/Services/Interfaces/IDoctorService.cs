using Docsm.DTOs.AppointmentDtos;
using Docsm.DTOs.DoctorDtos;
using Docsm.DTOs.ReviewDtos;

namespace Docsm.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorGetDto>> GetAllAsync();
        Task<DoctorGetDto> GetByIdAsync(int id);
        Task CreateOrUpdateAsync(DoctorCreateDto dto);
        Task DeleteAsync(int id);
        Task<List<ReviewGetDto>> GetDoctorDashboardReviewsAsync();
        Task<List<AppointmentGetDtoForDoctor>> GetDoctorAppointmentsAsync(int doctorId);
    }
}
