using System;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Services
{
    public class PaymentsService : IPaymentsService
    {
        public IUnprocessedPayment CreateUnprocessedPayment(IPaymentRequest paymentRequest) =>
            new UnprocessedPayment
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
                DateTime = DateTime.UtcNow
            };

        public IProcessedPayment CreateProcessedPayment(IUnprocessedPayment unprocessedPayment, IAcquiringBankResponse acquiringBankResponse)
        {
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
                DateTime = unprocessedPayment.DateTime
            };
            
            if (acquiringBankResponse == null) return processedPayment;
            
            processedPayment.Success = acquiringBankResponse.Success;
            processedPayment.AcquiringBankPaymentId = acquiringBankResponse.PaymentId;
            
            return processedPayment;
        }
    }
}