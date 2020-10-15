using System;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Models;
using PaymentGateway.Helpers;
using Xunit;

namespace PaymentGateway.Tests.API.Helpers
{
    public class PaymentDetailsDtoFactoryTests
    {
        private readonly PaymentDetailsDtoFactory _paymentDetailsDtoFactory;
        
        public PaymentDetailsDtoFactoryTests()
        {
            _paymentDetailsDtoFactory = new PaymentDetailsDtoFactory();
        }

        [Fact]
        public void CreatePaymentDetailsDto_WithValidPaymentDetails_ReturnsCorrectPaymentDetailsDto()
        {
            var paymentDetails = new PaymentDetails
            {
                CardNumber = "1234",
                Currency = Currency.EUR,
                CardCvv = 123,
                CardExpiryMonth = 12,
                CardExpiryYear = 2020,
                Success = true,
                UserId = "1234",
                Amount = 12,
                CreatedAt = DateTime.Now,
                Id = "1234"
            };

            var result = _paymentDetailsDtoFactory.CreatePaymentDetailsDto(paymentDetails);
            
            Assert.Equal(paymentDetails.Amount, result.Amount);
            Assert.Equal(paymentDetails.Currency, result.Currency);
            Assert.Equal(paymentDetails.Id, result.PaymentId);
            Assert.Equal(paymentDetails.Success, result.Success);
            Assert.Equal(paymentDetails.CreatedAt, result.PaymentCreatedAt);
            Assert.Equal(paymentDetails.CardCvv, result.CardDetails.Cvv);
            Assert.Equal(paymentDetails.CardExpiryMonth, result.CardDetails.ExpiryMonth);
            Assert.Equal(paymentDetails.CardExpiryYear, result.CardDetails.ExpiryYear);
        }
        
        [Theory]
        [InlineData("111122223333", "XXXXXXXX3333")]
        [InlineData("11112222", "XXXX2222")]
        [InlineData("12345678910", "XXXXXXX8910")]
        public void CreatePaymentDetailsDto_WithCardNumberLessThan13Digits_ReturnsCardNumberWithOnlyLast4DigitsUnMasked(string cardNumber, string expected)
        {
            var paymentDetails = new PaymentDetails
            {
                CardNumber = cardNumber
            };

            var result = _paymentDetailsDtoFactory.CreatePaymentDetailsDto(paymentDetails);
            
            Assert.Equal(expected, result.CardDetails.MaskedCardNumber);
        }
        
        [Theory]
        [InlineData("1111222233334444", "111122XXXXXX4444")]
        [InlineData("111122223333XXXX5555", "111122XXXXXXXXXX5555")]
        [InlineData("1234567891234", "123456XXX1234")]
        public void CreatePaymentDetailsDto_WithCardNumberGreaterThan12Digits_ReturnsCardNumberWithFirst6andLast4DigitsUnMasked(string cardNumber, string expected)
        {
            var paymentDetails = new PaymentDetails
            {
                CardNumber = cardNumber
            };

            var result = _paymentDetailsDtoFactory.CreatePaymentDetailsDto(paymentDetails);
            
            Assert.Equal(expected, result.CardDetails.MaskedCardNumber);
        }
        
    }
}