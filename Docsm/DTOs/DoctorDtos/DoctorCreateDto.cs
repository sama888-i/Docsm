using Docsm.Helpers.Enums;

namespace Docsm.DTOs.DoctorDtos
{
    public class DoctorCreateDto
    {
       
      
        public string PhoneNumber {  get; set; }
        public Genders  Gender {  get; set; }
        public string Surname {  get; set; }
        public string Name {  get; set; }
        public DateTime  DateOfBirth { get; set; }
        public string Adress {  get; set; }
        public decimal PerAppointPrice {  get; set; }
        public int? SpecialtyId { get; set; }
        public string? AboutMe { get; set; }
        public string? Services { get; set; }
        public string? ClinicName { get; set; }
        public IFormFile? Image { get; set; }

    }
}
