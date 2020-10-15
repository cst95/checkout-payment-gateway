using System.Threading.Tasks;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IPaymentsService
    {
        /// <summary>
        /// Create a new unprocessed payment from a payment request.
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        IUnprocessedPayment CreateUnprocessedPayment(IPaymentRequest paymentRequest);
        /// <summary>
        /// Take an unprocessed payment and and acquiring bank response and map it to a processed payment.
        /// </summary>
        /// <param name="unprocessedPayment"></param>
        /// <param name="acquiringBankResponse"></param>
        /// <returns></returns>
        IProcessedPayment CreateProcessedPayment(IUnprocessedPayment unprocessedPayment, IAcquiringBankResponse acquiringBankResponse);
        /// <summary>
        /// Retrieve a payment by it's paymentId
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        Task<PaymentDetails> GetPaymentByIdAsync(string paymentId);
    }
}