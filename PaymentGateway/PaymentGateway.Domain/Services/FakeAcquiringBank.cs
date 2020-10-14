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
            return Task.FromResult<IAcquiringBankResponse>(new FakeAcquiringBankResponse
            {
                PaymentId = Guid.NewGuid().ToString(),
                Success = true
            });
        }

        public Task<IAcquiringBankRequest> CreateRequestAsync(IUnprocessedPayment unprocessedPayment)
        {
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