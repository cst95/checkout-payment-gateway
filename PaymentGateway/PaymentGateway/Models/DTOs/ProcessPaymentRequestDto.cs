using PaymentGateway.Data.Models.Entities;

namespace PaymentGateway.Models.DTOs
{
    public class ProcessPaymentRequestDto
    {
        public string CardNumber { get; set; }
        public int? CardExpiryMonth { get; set; }
        public int? CardExpiryYear { get; set; }
        public int? Cvv { get; set; }
        public decimal? Amount { get; set; }
        public Currency? Currency { get; set; }
    }
}