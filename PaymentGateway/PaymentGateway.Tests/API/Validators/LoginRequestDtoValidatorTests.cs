using FluentValidation.TestHelper;
using PaymentGateway.Models.DTOs;
using PaymentGateway.Validators;
using Xunit;

namespace PaymentGateway.Tests.API.Validators
{
    public class LoginRequestDtoValidatorTests
    {
        private LoginRequestDtoValidator _loginRequestDtoValidator;
        
        public LoginRequestDtoValidatorTests()
        {
            _loginRequestDtoValidator = new LoginRequestDtoValidator();
        }

        [Fact]
        public void Validate_WhenValuesAreNull_ShouldError()
        {
            var request = new LoginRequestDto();

            var result = _loginRequestDtoValidator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Username);
            result.ShouldHaveValidationErrorFor(r => r.Password);
        }
        
        [Fact]
        public void Validate_WhenValuesAreNotNull_ShouldNotError()
        {
            var request = new LoginRequestDto
            {
                Username = "username",
                Password = "password"
            };

            var result = _loginRequestDtoValidator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(r => r.Username);
            result.ShouldNotHaveValidationErrorFor(r => r.Password);
        }
    }
}