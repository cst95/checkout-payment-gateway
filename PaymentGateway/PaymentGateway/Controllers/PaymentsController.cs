using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Models;
using PaymentGateway.Models.DTOs;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPaymentProcessor _paymentProcessor;

        public PaymentsController(UserManager<User> userManager, IPaymentProcessor paymentProcessor)
        {
            _userManager = userManager;
            _paymentProcessor = paymentProcessor;
        }

        [HttpPost]
        public async Task<ActionResult<ProcessPaymentResponseDto>> ProcessPayment(
            [FromBody] ProcessPaymentRequestDto paymentRequestDto)
        {
            // TODO: validate paymentRequestDto
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var paymentResult = await _paymentProcessor.ProcessAsync(new PaymentRequest
            {
                Amount = paymentRequestDto.Amount,
                Currency = paymentRequestDto.Currency,
                CardCvv = paymentRequestDto.Cvv,
                CardNumber = paymentRequestDto.CardNumber,
                CardExpiryMonth = paymentRequestDto.CardExpiryMonth,
                CardExpiryYear = paymentRequestDto.CardExpiryYear,
                User = user
            });

            return Ok(new ProcessPaymentResponseDto
            {
                PaymentId = paymentResult.Payment.Id,
                Success = paymentResult.Payment.Success
            });
        }
    }
}