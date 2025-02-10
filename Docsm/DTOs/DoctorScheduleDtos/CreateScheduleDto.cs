
using System.Text.Json.Serialization;

namespace Docsm.DTOs.DoctorScheduleDtos
{
    public class CreateScheduleDto
    {
        public int DoctorId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
