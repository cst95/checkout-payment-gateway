using System.Threading.Tasks;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IPaymentsService
    {
        IUnprocessedPayment CreateUnprocessedPayment(IPaymentRequest paymentRequest);
        IProcessedPayment CreateProcessedPayment(IUnprocessedPayment unprocessedPayment, IAcquiringBankResponse acquiringBankResponse);
        Task<PaymentDetails> GetPaymentByIdAsync(string paymentId);
    }
}