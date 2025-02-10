using Docsm.Models.Common;

namespace Docsm.Models
{
    public class DoctorTimeSchedule:BaseEntity 
    {
        public int DoctorId {  get; set; }
        public Doctor Doctor { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
