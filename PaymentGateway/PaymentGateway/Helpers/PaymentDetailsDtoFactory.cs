using PaymentGateway.Domain.Models;
using PaymentGateway.Models.DTOs;

namespace PaymentGateway.Helpers
{
    public class PaymentDetailsDtoFactory : IPaymentDetailsDtoFactory
    {
        private const int DigitsAllowedAtStartOfCardNumber = 6;
        private const int DigitsAllowedAtEndOfCardNumber = 4;
        private const char MaskChar = 'X';
        
        public PaymentDetailsDto CreatePaymentDetailsDto(PaymentDetails payment)
            => new PaymentDetailsDto
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Success = payment.Success,
                PaymentCreatedAt = payment.DateTime,
                CardDetails = new CardDetailsDto
                {
                    MaskedCardNumber = MaskCardNumber(payment.CardNumber),
                    Cvv = payment.CardCvv,
                    ExpiryMonth = payment.CardExpiryMonth,
                    ExpiryYear = payment.CardExpiryYear
                }
            };

        private string MaskCardNumber(string cardNumber)
        {
            var maskLength = cardNumber.Length - DigitsAllowedAtEndOfCardNumber - DigitsAllowedAtStartOfCardNumber;
            var mask = new string(MaskChar, maskLength);
            var start = cardNumber.Substring(0, DigitsAllowedAtStartOfCardNumber);
            var end = cardNumber.Substring(cardNumber.Length - DigitsAllowedAtEndOfCardNumber, DigitsAllowedAtEndOfCardNumber);

            return start + mask + end;
        }
    }
    
    
}