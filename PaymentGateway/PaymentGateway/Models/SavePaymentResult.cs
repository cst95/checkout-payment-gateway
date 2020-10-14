using PaymentGateway.API.Interfaces;

namespace PaymentGateway.API.Models
{
    public class SavePaymentResult
    {
        /// <summary>
        /// The resulting payment object.
        /// </summary>
        public IProcessedPayment Payment { get; set; }
        /// <summary>
        /// Indicates whether the payment was successfully saved.
        /// </summary>
        public bool Success { get; set; }
    }
}