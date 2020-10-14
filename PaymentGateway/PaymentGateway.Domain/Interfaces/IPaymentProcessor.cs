using System.Threading.Tasks;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Process the given payment request.
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        Task<ProcessPaymentResult> ProcessAsync(IPaymentRequest paymentRequest);
    }
}