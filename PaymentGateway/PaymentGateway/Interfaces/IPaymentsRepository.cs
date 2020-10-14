using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Interfaces
{
    public interface IPaymentsRepository
    {
        /// <summary>
        /// Save the payment to the backing store. 
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        Task<SavePaymentResult> SaveProcessedPaymentAsync(IProcessedPayment payment);
    }
}