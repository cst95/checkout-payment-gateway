using System.Threading.Tasks;

namespace PaymentGateway.API.Interfaces
{
    public interface IAcquiringBank
    {
        /// <summary>
        /// Send a payment request to the acquiring bank.
        /// </summary>
        /// <param name="request">The payment request sent to the acquiring bank.</param>
        /// <returns></returns>
        Task<IAcquiringBankResponse> ProcessPaymentAsync(IAcquiringBankRequest request);
    }
}