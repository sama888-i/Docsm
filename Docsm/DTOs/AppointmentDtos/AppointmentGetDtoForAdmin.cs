using Docsm.Helpers.Enums.Status;

namespace Docsm.DTOs.AppointmentDtos
{
    public class AppointmentGetDtoForAdmin
    {
        public string DoctorName { get; set; }
        public string? DoctorImage { get; set; }
        public string PatientName { get; set; }
        public string? PatientImage { get; set; }
        public DateOnly  AppointmentDate { get; set; }
        public decimal Amount { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
