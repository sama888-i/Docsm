using Docsm.Helpers.Enums.Status;

namespace Docsm.DTOs.AppointmentDtos
{
    public class AppointmentGetDtoForDoctor
    {
        public string PatientName { get; set; }
        public string? PatientImage { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public decimal Amount { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
