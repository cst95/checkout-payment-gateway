using PaymentGateway.API.Interfaces;

namespace PaymentGateway.API.Models
{
    public class FakeAcquiringBankResponse : IAcquiringBankResponse
    {
        public string PaymentId { get; set; }
        public bool Success { get; set; }
    }
}