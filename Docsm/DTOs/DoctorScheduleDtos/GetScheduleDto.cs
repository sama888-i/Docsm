namespace Docsm.DTOs.DoctorScheduleDtos
{
    public class GetScheduleDto
    {
        public int Id { get; set; }
        public int DoctorId {  get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public bool IsAvailable { get;set; }

    }
}
