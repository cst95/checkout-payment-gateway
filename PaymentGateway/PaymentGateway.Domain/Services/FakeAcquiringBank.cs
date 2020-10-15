using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Services
{
    public class FakeAcquiringBank : IAcquiringBank
    {
        private readonly ILogger<FakeAcquiringBank> _logger;

        public FakeAcquiringBank(ILogger<FakeAcquiringBank> logger)
        {
            _logger = logger;
        }
        
        public Task<IAcquiringBankResponse> ProcessPaymentAsync(IAcquiringBankRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var paymentId = Guid.NewGuid().ToString();
            var success = request.Amount < 500;
            
            _logger.LogInformation("The fake acquiring bank created a payment {AcquiringBankPaymentId} with success: {Success}.", paymentId, success);
            
            return Task.FromResult<IAcquiringBankResponse>(new FakeAcquiringBankResponse
            {
                PaymentId = paymentId,
                Success = success
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