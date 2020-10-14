using PaymentGateway.Interfaces;
using PaymentGateway.Models.Entities;

namespace PaymentGateway.Models
{
    public class PaymentRequest : IAcquiringBankRequest, IPaymentRequest
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