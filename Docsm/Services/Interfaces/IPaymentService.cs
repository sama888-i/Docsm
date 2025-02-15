using Docsm.DTOs.AppointmentDtos;

namespace Docsm.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentMethodWithToken(string token);
        Task<string> CreatePaymentIntent(decimal amount, string currency, string paymentMethodId);
        Task<string> ProcessPayment(CardDetailsDto dto, decimal amount, string currency);
        Task CapturePayment(string paymentIntentId);
        Task RefundPayment(string paymentIntentId);
    }
}
