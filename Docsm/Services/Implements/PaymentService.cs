using Docsm.DTOs.AppointmentDtos;
using Docsm.Options;
using Docsm.Services.Interfaces;
using Microsoft.Extensions.Options;
using Stripe;

namespace Docsm.Services.Implements
{
    public class PaymentService:IPaymentService 
    {
          private readonly StripeOptions _opt;
        public PaymentService(IOptions<StripeOptions> options)
        {
            _opt = options.Value;
            StripeConfiguration.ApiKey = _opt.SecretKey;
        }
        public async Task<string> CreatePaymentMethod(CardDetailsDto dto)
        {
            var options = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Number = dto.CardNumber,
                    ExpMonth = dto.ExpiryMonth,
                    ExpYear = dto.ExpiryYear,
                    Cvc = dto.Cvv
                }
            };

            var service = new PaymentMethodService();
            var paymentMethod = await service.CreateAsync(options);
            return paymentMethod.Id;
        }
        public async Task<string> CreatePaymentIntent(decimal amount, string currency, string paymentMethodId)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), 
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" },
                CaptureMethod = "manual", 
                PaymentMethod = paymentMethodId,
                Confirm = true 
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);
            return intent.Id;
        }
        public async Task<string> ProcessPayment(CardDetailsDto dto, decimal amount, string currency)
        {
            var paymentMethodId = await CreatePaymentMethod(dto);
            var paymentIntentId = await CreatePaymentIntent(amount, currency, paymentMethodId);
            return paymentIntentId;
        }
        public async Task CapturePayment(string paymentIntentId)
        {
            var service = new PaymentIntentService();
            await service.CaptureAsync(paymentIntentId);
        }
        public async Task RefundPayment(string paymentIntentId)
        {
            var refundService = new RefundService();
            var options = new RefundCreateOptions { PaymentIntent = paymentIntentId };
            await refundService.CreateAsync(options);
        }
    }
}
