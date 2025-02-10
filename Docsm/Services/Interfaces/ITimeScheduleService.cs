using Docsm.DTOs.DoctorScheduleDtos;

namespace Docsm.Services.Interfaces
{
    public interface ITimeScheduleService
    {
        Task CreateScheduleAsync(CreateScheduleDto dto);
        Task<List<GetScheduleDto>> GetAllSchedulesAsync(int doctorId);
        Task DeleteScheduleAsync(int scheduleId);
        Task UpdateScheduleAsync(int scheduleId, UpdateScheduleDto  dto);

    }
}
