using FluentValidation;
using PaymentGateway.Models.DTOs;

namespace PaymentGateway.Validators
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(r => r.Username).NotNull();
            RuleFor(r => r.Password).NotNull();
        }
    }
}