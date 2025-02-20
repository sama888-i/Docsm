namespace Docsm.DTOs.DoctorScheduleDtos
{
    public class UpdateScheduleDto
    {
        public int DoctorId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly startTime { get; set; }
        public TimeOnly endTime { get; set; }
    }
}
