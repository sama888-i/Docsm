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
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
