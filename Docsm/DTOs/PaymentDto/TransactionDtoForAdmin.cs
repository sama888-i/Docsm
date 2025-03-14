namespace Docsm.DTOs.PaymentDto
{
    public class TransactionDtoForAdmin
    {
        public int TransactionId { get; set; }
        public string PatientName { get; set; }
        public string? PatientImage { get; set; }
        public string DoctorName { get; set; }
        public string? DoctorImage { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentStatus { get; set; }
        public DateOnly AppointmentDate { get; set; }

    }
}
