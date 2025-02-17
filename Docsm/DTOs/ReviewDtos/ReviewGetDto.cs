namespace Docsm.DTOs.ReviewDtos
{
    public class ReviewGetDto
    {
        public int Id { get; set; }
        public string PatientFullname { get; set; }
        public string PatientImage { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
        public List<ReviewGetDto> Replies { get; set; } = new List<ReviewGetDto>();


    }
}
