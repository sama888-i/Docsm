using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Docsm.DTOs.ReviewDtos
{
    public class AdminReviewDto
    {
        public int Id { get; set; }
        public string DoctorFullname { get; set; }
        public string DoctorImage { get; set; }
        public string PatientFullname { get; set; }
        public string PatientImage { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
        public int? ParentId { get; set; }
    }
}
