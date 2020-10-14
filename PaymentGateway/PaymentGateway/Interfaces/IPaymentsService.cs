using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Interfaces
{
    public interface IPaymentsService
    {
        IUnprocessedPayment CreateUnprocessedPayment(IPaymentRequest paymentRequest);
        IProcessedPayment CreateProcessedPayment(IUnprocessedPayment unprocessedPayment, IAcquiringBankResponse acquiringBankResponse);
    }
}