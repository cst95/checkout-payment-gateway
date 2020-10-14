using System.Threading.Tasks;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
    public interface IPaymentProcessor
    {
        Task<ProcessPaymentResult> ProcessAsync(IPaymentRequest paymentRequest);
    }
}