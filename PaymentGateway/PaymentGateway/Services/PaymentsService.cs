using System.Threading.Tasks;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IAcquiringBank _acquiringBank;

        public PaymentsService(IPaymentsRepository paymentsRepository, IAcquiringBank acquiringBank)
        {
            _paymentsRepository = paymentsRepository;
            _acquiringBank = acquiringBank;
        }
        
        public async Task<ProcessPaymentResult> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            // TODO: Implement some decision here as to which acquiring bank implementation is used. Perhaps use factory pattern.
            var acquiringBankResponse = await _acquiringBank.ProcessPaymentAsync(paymentRequest);
            
            var saveResult = await _paymentsRepository.SavePaymentAsync(new SavePaymentRequest
            {
                PaymentRequest = paymentRequest,
                AcquiringBankResponse = acquiringBankResponse
            });
            
            return new ProcessPaymentResult
            {
                Payment = saveResult.Payment
            };
        }
    }
}