using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Models
{
    public class PaymentRequest : IPaymentRequest
    {
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public int CardCvv { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public User User { get; set; }
    }
}