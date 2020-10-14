using PaymentGateway.Data.Models.Entities;

namespace PaymentGateway.Data.Models
{
    public class SavePaymentResult
    {
        /// <summary>
        /// The resulting payment object.
        /// </summary>
        public Payment Payment { get; set; }
        /// <summary>
        /// Indicates whether the payment was successfully saved.
        /// </summary>
        public bool Success { get; set; }
    }
}