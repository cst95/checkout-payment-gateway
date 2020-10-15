using FluentValidation;
using PaymentGateway.Models.DTOs;

namespace PaymentGateway.Validators
{
    public class ProcessPaymentRequestDtoValidator : AbstractValidator<ProcessPaymentRequestDto>
    {
        public ProcessPaymentRequestDtoValidator()
        {
            RuleFor(req => req.CardNumber).NotNull().CreditCard();
            RuleFor(req => req.Amount).NotNull().GreaterThan(0);
            RuleFor(req => req.Currency).NotNull().IsInEnum();
            RuleFor(req => req.Cvv).NotNull();
            RuleFor(req => req.Cvv).GreaterThan(99).LessThan(10000).WithMessage("CVV must be either 3 or 4 digits.");
            RuleFor(req => req.CardExpiryMonth).NotNull().GreaterThan(0).LessThanOrEqualTo(12);
            RuleFor(req => req.CardExpiryYear).NotNull();
        }
    }
}