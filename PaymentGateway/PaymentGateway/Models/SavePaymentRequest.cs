using PaymentGateway.Interfaces;

namespace PaymentGateway.Models
{
    public class SavePaymentRequest
    {
        /// <summary>
        /// The response from the acquiring bank.
        /// </summary>
        public IAcquiringBankResponse AcquiringBankResponse { get; set; }
        /// <summary>
        /// The request containing details of the payment.
        /// </summary>
        public PaymentRequest PaymentRequest { get; set; }
    }
}