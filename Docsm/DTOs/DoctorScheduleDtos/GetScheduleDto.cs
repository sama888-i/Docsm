namespace Docsm.DTOs.DoctorScheduleDtos
{
    public class GetScheduleDto
    {
        public int Id { get; set; }
        public int DoctorId {  get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly startTime { get; set; }
        public TimeOnly endTime { get; set; }
        public bool IsAvailable { get;set; }

    }
}
