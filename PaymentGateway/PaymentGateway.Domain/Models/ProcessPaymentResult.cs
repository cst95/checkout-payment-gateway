using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Domain.Models
{
    public class ProcessPaymentResult
    {
        /// <summary>
        /// Details of the processed payment.
        /// </summary>
        public IProcessedPayment Payment { get; set; }
    }
}