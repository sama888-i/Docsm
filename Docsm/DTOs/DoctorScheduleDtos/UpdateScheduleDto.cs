namespace Docsm.DTOs.DoctorScheduleDtos
{
    public class UpdateScheduleDto
    {
        public int DoctorId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
