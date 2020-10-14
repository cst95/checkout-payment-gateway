using System.Threading.Tasks;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Interfaces
{
    public interface IPaymentsService
    {
        IUnprocessedPayment CreateUnprocessedPayment(IPaymentRequest paymentRequest);
        IProcessedPayment CreateProcessedPayment(IUnprocessedPayment unprocessedPayment, IAcquiringBankResponse acquiringBankResponse);
    }
}