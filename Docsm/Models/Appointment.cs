using Docsm.Helpers.Enums.Status;
using Docsm.Models.Common;

namespace Docsm.Models
{
    public class Appointment:BaseEntity 
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int DoctorScheduleId {  get; set; }
        public DoctorTimeSchedule DoctorTimeSchedule { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? ReasonAppointment {  get; set; }
    }
}
