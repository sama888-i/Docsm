using Docsm.DTOs.AppointmentDtos;

namespace Docsm.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<string> CreateAppointmentAsync(int patientId, AppointmentCreateDto dto);
        Task<string> ConfirmAppointmentAsync(int appointmentId);
        Task<string> CancelAppointmentAsync(int appointmentId);
    }
}
