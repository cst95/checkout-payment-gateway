using PaymentGateway.Domain.Models;
using PaymentGateway.Models.DTOs;

namespace PaymentGateway.Helpers
{
    public interface IPaymentDetailsDtoFactory
    {
        PaymentDetailsDto CreatePaymentDetailsDto(PaymentDetails payment);
    }
}