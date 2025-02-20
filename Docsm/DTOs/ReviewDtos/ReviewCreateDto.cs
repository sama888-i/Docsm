namespace Docsm.DTOs.ReviewDtos
{
    public class ReviewCreateDto
    {
        public int DoctorId { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
       
    }
}
