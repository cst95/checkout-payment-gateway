using System.Threading.Tasks;
using PaymentGateway.API.Models;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Interfaces
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