using System.Threading.Tasks;
using PaymentGateway.Data.Models;
using PaymentGateway.Data.Models.Entities;

namespace PaymentGateway.Data.Interfaces
{
    public interface IPaymentsRepository
    {
        /// <summary>
        /// Save the payment to the backing store. 
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        Task<SavePaymentResult> SavePaymentAsync(Payment payment);
    }
}