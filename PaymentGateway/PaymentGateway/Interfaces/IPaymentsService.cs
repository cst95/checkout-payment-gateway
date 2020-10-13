using System.Threading.Tasks;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Interfaces
{
    public interface IPaymentsService
    {
        /// <summary>
        /// Makes the payment gateway process the payment.
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        Task<ProcessPaymentResult> ProcessPaymentAsync(PaymentRequest paymentRequest);
    }
}