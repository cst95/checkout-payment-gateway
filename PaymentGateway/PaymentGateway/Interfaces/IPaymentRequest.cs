using PaymentGateway.Data.Models.Entities;

namespace PaymentGateway.Interfaces
{
    public interface IPaymentRequest
    {
        string CardNumber { get; set; }
        int CardExpiryMonth { get; set; }
        int CardExpiryYear { get; set; }
        int CardCvv { get; set; }
        decimal Amount { get; set; }
        Currency Currency { get; set; }
        User User { get; set; }
    }
}