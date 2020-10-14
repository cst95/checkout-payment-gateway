using System.Threading.Tasks;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Services
{
    public interface IPaymentProcessor
    {
        Task<ProcessPaymentResult> ProcessAsync(IPaymentRequest paymentRequest);
    }
}