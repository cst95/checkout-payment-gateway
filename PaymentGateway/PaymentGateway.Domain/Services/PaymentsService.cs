using System;
using System.Threading.Tasks;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _paymentsRepository;

        public PaymentsService(IPaymentsRepository paymentsRepository)
        {
            _paymentsRepository = paymentsRepository;
        }
        
        public IUnprocessedPayment CreateUnprocessedPayment(IPaymentRequest paymentRequest)
        {
            if (paymentRequest == null) throw new ArgumentNullException(nameof(paymentRequest));
            
            if(paymentRequest.User == null) throw new ArgumentException($"User property of {nameof(paymentRequest)} cannot be null");
            
            return new UnprocessedPayment
            {
                Id = Guid.NewGuid().ToString(),
                CardNumber = paymentRequest.CardNumber,
                CardExpiryMonth = paymentRequest.CardExpiryMonth,
                CardExpiryYear = paymentRequest.CardExpiryYear,
                CardCvv = paymentRequest.CardCvv,
                Amount = paymentRequest.Amount,
                Currency = paymentRequest.Currency,
                UserId = paymentRequest.User.Id,
                User = paymentRequest.User,
                CreatedAt = DateTime.UtcNow
            };
        }

        public IProcessedPayment CreateProcessedPayment(IUnprocessedPayment unprocessedPayment, IAcquiringBankResponse acquiringBankResponse)
        {
            if (unprocessedPayment == null) throw new ArgumentNullException(nameof(unprocessedPayment));
            
            var processedPayment = new ProcessedPayment
            {
                Id = unprocessedPayment.Id,
                CardNumber = unprocessedPayment.CardNumber,
                CardCvv = unprocessedPayment.CardCvv,
                CardExpiryMonth = unprocessedPayment.CardExpiryMonth,
                CardExpiryYear = unprocessedPayment.CardExpiryYear,
                Amount = unprocessedPayment.Amount,
                Currency = unprocessedPayment.Currency,
                UserId = unprocessedPayment.UserId,
                User = unprocessedPayment.User,
                CreatedAt = unprocessedPayment.CreatedAt
            };
            
            if (acquiringBankResponse == null) return processedPayment;
            
            processedPayment.Success = acquiringBankResponse.Success;
            processedPayment.AcquiringBankPaymentId = acquiringBankResponse.PaymentId;
            
            return processedPayment;
        }

        public async Task<PaymentDetails> GetPaymentByIdAsync(string paymentId)
        {
            var result = await _paymentsRepository.GetPaymentByIdAsync(paymentId);

            if (result == null) return null;

            return new PaymentDetails
            {
                Amount = result.Amount,
                CardCvv = result.CardCvv,
                CardExpiryMonth = result.CardExpiryMonth,
                CardExpiryYear = result.CardExpiryYear,
                CardNumber = result.CardNumber,
                Currency = result.Currency,
                CreatedAt = result.CreatedAt,
                Id = result.Id,
                Success = result.Success,
                UserId = result.UserId
            };
        }
    }
}