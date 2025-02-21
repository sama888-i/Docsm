
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Docsm.DTOs.DoctorScheduleDtos
{
    public class CreateScheduleDto
    {
        public int DoctorId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly  startTime { get; set; }
        public TimeOnly  endTime { get; set; }
    }
}
