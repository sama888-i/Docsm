using Docsm.Helpers.Enums.Status;
using Docsm.Models.Common;

namespace Docsm.Models
{
    public class Payment:BaseEntity
    {
        public string PaymentIntentId {  get; set; }
        public decimal Amount { get; set; }
        public string Currency {  get; set; }
        public int AppointmentId {  get; set; }
        public Appointment Appointment { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

    }
}
