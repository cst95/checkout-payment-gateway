using PaymentGateway.Domain.Models;
using PaymentGateway.Interfaces;
using PaymentGateway.Models.DTOs;

namespace PaymentGateway.Helpers
{
    public class PaymentDetailsDtoFactory : IPaymentDetailsDtoFactory
    {
        private const int DigitsAllowedAtStartOfCardNumber = 6;
        private const int DigitsAllowedAtEndOfCardNumber = 4;
        private const int LowerLimitBeforeOnlyUnMaskingLast4Digits = 13;
        
        private const char MaskChar = 'X';
        
        public PaymentDetailsDto CreatePaymentDetailsDto(PaymentDetails payment)
            => new PaymentDetailsDto
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Success = payment.Success,
                PaymentCreatedAt = payment.CreatedAt,
                CardDetails = GetCardDetailsDto(payment)
            };

        private CardDetailsDto GetCardDetailsDto(PaymentDetails payment) => new CardDetailsDto
        {
            MaskedCardNumber = GetMaskedCardNumber(payment.CardNumber),
            Cvv = payment.CardCvv,
            ExpiryMonth = payment.CardExpiryMonth,
            ExpiryYear = payment.CardExpiryYear
        };

        private string GetMaskedCardNumber(string cardNumber) =>
            cardNumber.Length < LowerLimitBeforeOnlyUnMaskingLast4Digits
                ? MaskCardNumber(cardNumber, 0, DigitsAllowedAtEndOfCardNumber)
                : MaskCardNumber(cardNumber, DigitsAllowedAtStartOfCardNumber, DigitsAllowedAtEndOfCardNumber);
        
        private string MaskCardNumber(string cardNumber, int digitsAllowedAtStart, int digitsAllowedAtEnd)
        {
            var maskLength = cardNumber.Length - digitsAllowedAtStart - digitsAllowedAtEnd;
            var mask = new string(MaskChar, maskLength);
            var start = cardNumber.Substring(0, digitsAllowedAtStart);
            var end = cardNumber.Substring(cardNumber.Length - digitsAllowedAtEnd, digitsAllowedAtEnd);

            return start + mask + end;
        }
    }
    
    
}