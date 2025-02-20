using Docsm.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Docsm.Models
{
    public class DoctorTimeSchedule:BaseEntity 
    {
        public int DoctorId {  get; set; }
        public Doctor Doctor { get; set; }
        public DateOnly  AppointmentDate { get; set; }
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }
        public bool IsAvailable { get; set; } = true;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
