using PaymentGateway.Domain.Models;
using PaymentGateway.Models.DTOs;

namespace PaymentGateway.Interfaces
{
    public interface IPaymentDetailsDtoFactory
    {
        /// <summary>
        /// Create the payment details DTO, with card details that have been sufficiently masked.
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        PaymentDetailsDto CreatePaymentDetailsDto(PaymentDetails payment);
    }
}