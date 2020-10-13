using System;
using System.Threading.Tasks;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Services
{
    public class FakeAcquiringBank : IAcquiringBank
    {
        public Task<IAcquiringBankResponse> ProcessPaymentAsync(IAcquiringBankRequest request)
        {
            return Task.FromResult<IAcquiringBankResponse>(new FakeAcquiringBankResponse
            {
                Id = Guid.NewGuid().ToString(),
                Success = true
            });
        }
    }
}