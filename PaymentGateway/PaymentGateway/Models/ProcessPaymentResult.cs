using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Models
{
    public class ProcessPaymentResult
    {
        /// <summary>
        /// Details of the payment along with a success indicator.
        /// </summary>
        public Payment Payment { get; set; }
    }
}