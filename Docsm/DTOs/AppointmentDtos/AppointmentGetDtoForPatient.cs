using Docsm.Helpers.Enums.Status;

namespace Docsm.DTOs.AppointmentDtos
{
    public class AppointmentGetDtoForPatient
    {
        public string DoctorName { get; set; }
        public string? DoctorImage { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public decimal Amount { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
