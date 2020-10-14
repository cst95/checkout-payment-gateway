using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Domain.Models
{
    public class FakeAcquiringBankResponse : IAcquiringBankResponse
    {
        public string PaymentId { get; set; }
        public bool Success { get; set; }
    }
}