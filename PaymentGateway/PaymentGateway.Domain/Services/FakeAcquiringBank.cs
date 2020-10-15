using System;
using System.Threading.Tasks;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Services
{
    public class FakeAcquiringBank : IAcquiringBank
    {
        public Task<IAcquiringBankResponse> ProcessPaymentAsync(IAcquiringBankRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            return Task.FromResult<IAcquiringBankResponse>(new FakeAcquiringBankResponse
            {
                PaymentId = Guid.NewGuid().ToString(),
                Success = request.Amount < 500
            });
        }

        public Task<IAcquiringBankRequest> CreateRequestAsync(IPaymentRequest unprocessedPayment)
        {
            if (unprocessedPayment == null) throw new ArgumentNullException(nameof(unprocessedPayment));

            return Task.FromResult<IAcquiringBankRequest>(new FakeAcquiringBankRequest
            {
                CardNumber = unprocessedPayment.CardNumber,
                CardCvv = unprocessedPayment.CardCvv,
                Amount = unprocessedPayment.Amount,
                CardExpiryMonth = unprocessedPayment.CardExpiryMonth,
                CardExpiryYear = unprocessedPayment.CardExpiryYear,
                Currency = unprocessedPayment.Currency
            });
        }
    }
}