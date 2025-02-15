namespace Docsm.DTOs.AppointmentDtos
{
    public class CardDetailsDto
    {
        public string CardNumber { get; set; }
        public long ExpiryMonth { get; set; }
        public long ExpiryYear { get; set; }
        public string Cvv { get; set; }
    }
}
