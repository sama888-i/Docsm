namespace Docsm.DTOs.AppointmentDtos
{
    public class AppointmentCreateDto
    {
        public int DoctorId { get; set; }
        public int DoctorScheduleId { get; set; }
        public string? ReasonAppointment {  get; set; }
        public CardDetailsDto CardDetails { get; set; }
    }
}
