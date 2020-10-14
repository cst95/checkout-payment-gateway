using PaymentGateway.API.Interfaces;

namespace PaymentGateway.API.Models
{
    public class ProcessPaymentResult
    {
        /// <summary>
        /// Details of the processed payment.
        /// </summary>
        public IProcessedPayment Payment { get; set; }
    }
}