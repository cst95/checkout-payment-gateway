using System.Threading.Tasks;

namespace PaymentGateway.Interfaces
{
    public interface IAcquiringBank
    {
        /// <summary>
        /// Send a payment request to the acquiring bank.
        /// </summary>
        /// <param name="request">The payment request sent to the acquiring bank.</param>
        /// <returns></returns>
        Task<IAcquiringBankResponse> ProcessPaymentAsync(IAcquiringBankRequest request);

        /// <summary>
        /// Take an unprocessed payment and create the required acquiring bank request.
        /// </summary>
        /// <param name="unprocessedPayment"></param>
        /// <returns></returns>
        Task<IAcquiringBankRequest> CreateRequestAsync(IUnprocessedPayment unprocessedPayment);
    }
}