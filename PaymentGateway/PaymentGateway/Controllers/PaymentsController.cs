using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Models;
using PaymentGateway.API.Models.DTOs;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsService _paymentsService;
        private readonly UserManager<User> _userManager;

        public PaymentsController(IPaymentsService paymentsService, UserManager<User> userManager)
        {
            _paymentsService = paymentsService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<ProcessPaymentResponseDto>> ProcessPayment(
            [FromBody] ProcessPaymentRequestDto paymentRequestDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var paymentResult = await _paymentsService.ProcessPaymentAsync(new PaymentRequest
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