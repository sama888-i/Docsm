namespace Docsm.DTOs.DoctorDtos
{
    public class SearchDoctorDto
    {
        public int DoctorId {  get; set; }
        public string DoctorName { get; set; }
        public string? DoctorImage { get; set; }
        public string Specialty { get; set; }
        public string? ClinicAddress { get; set; }
        public int? Rating { get; set; }
        public decimal PricePerAppointment {  get; set; }

    }
}
