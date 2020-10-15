using FluentValidation.TestHelper;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Models.DTOs;
using PaymentGateway.Validators;
using Xunit;

namespace PaymentGateway.Tests.API.Validators
{
    public class ProcessPaymentRequestDtoValidatorTests
    {
        private readonly ProcessPaymentRequestDtoValidator _validator;
        
        public ProcessPaymentRequestDtoValidatorTests()
        {
            _validator = new ProcessPaymentRequestDtoValidator();
        }

        [Fact]
        public void Validate_WhenValuesAreValid_ShouldNotError()
        {
            var request = new ProcessPaymentRequestDto
            {
                Currency = Currency.USD,
                CardNumber = "1111222233334444",
                CardExpiryMonth = 12,
                CardExpiryYear = 2020,
                Cvv = 123,
                Amount = 12
            };
            
            var result = _validator.TestValidate(request);
            
            result.ShouldNotHaveValidationErrorFor(p => p.Amount);
            result.ShouldNotHaveValidationErrorFor(p => p.Currency);
            result.ShouldNotHaveValidationErrorFor(p => p.Cvv);
            result.ShouldNotHaveValidationErrorFor(p => p.CardNumber);
            result.ShouldNotHaveValidationErrorFor(p => p.CardExpiryMonth);
            result.ShouldNotHaveValidationErrorFor(p => p.CardExpiryYear);
        }

        [Fact]
        public void Validate_WhenValuesAreNull_ShouldError()
        {
            var request = new ProcessPaymentRequestDto();
            
            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(p => p.Amount);
            result.ShouldHaveValidationErrorFor(p => p.Currency);
            result.ShouldHaveValidationErrorFor(p => p.Cvv);
            result.ShouldHaveValidationErrorFor(p => p.CardNumber);
            result.ShouldHaveValidationErrorFor(p => p.CardExpiryMonth);
            result.ShouldHaveValidationErrorFor(p => p.CardExpiryYear);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public void Validate_WhenExpiryMonthIsNotValid_ShouldError(int expiryMonth)
        {
            var request = new ProcessPaymentRequestDto {CardExpiryMonth = expiryMonth};
            
            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(p => p.Amount);
        }
        
        [Theory]
        [InlineData(22)]
        [InlineData(55555)]
        public void Validate_WhenCvvIsInvalid_ShouldError(int cvv)
        {
            var request = new ProcessPaymentRequestDto {Cvv = cvv};
            
            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(p => p.Cvv);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Validate_WhenAmountIsLessThanOrEqualsToZero_ShouldError(decimal amount)
        {
            var request = new ProcessPaymentRequestDto{ Amount = amount};
            
            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(p => p.Amount);
        }
    }
}