using PaymentGateway.Interfaces;

namespace PaymentGateway.Models
{
    public class FakeAcquiringBankResponse : IAcquiringBankResponse
    {
        public string PaymentId { get; set; }
        public bool Success { get; set; }
    }
}