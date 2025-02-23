namespace Docsm.DTOs.PatientDtos
{
    public class GetAllPatientsForAdmin
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string? PatientImage {  get; set; }
        public string Address {  get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

    }
}
