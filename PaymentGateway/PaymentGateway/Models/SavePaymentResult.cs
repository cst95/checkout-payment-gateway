using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Models
{
    public class SavePaymentResult
    {
        public bool Success { get; set; }
        public Payment Payment { get; set; }
    }
}