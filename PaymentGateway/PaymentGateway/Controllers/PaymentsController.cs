using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Models.DTOs;

namespace PaymentGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IPaymentsService _paymentsService;
        private readonly IPaymentDetailsDtoFactory _detailsDtoFactory;

        public PaymentsController(UserManager<User> userManager, IPaymentProcessor paymentProcessor,
            IPaymentsService paymentsService, IPaymentDetailsDtoFactory detailsDtoFactory)
        {
            _userManager = userManager;
            _paymentProcessor = paymentProcessor;
            _paymentsService = paymentsService;
            _detailsDtoFactory = detailsDtoFactory;
        }

        [HttpPost]
        public async Task<ActionResult<ProcessPaymentResponseDto>> ProcessPayment(
            [FromBody] ProcessPaymentRequestDto paymentRequestDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var paymentResult = await _paymentProcessor.ProcessAsync(new PaymentRequest
            {
                Amount = paymentRequestDto.Amount.Value,
                Currency = paymentRequestDto.Currency.Value,
                CardCvv = paymentRequestDto.Cvv.Value,
                CardNumber = paymentRequestDto.CardNumber,
                CardExpiryMonth = paymentRequestDto.CardExpiryMonth.Value,
                CardExpiryYear = paymentRequestDto.CardExpiryYear.Value,
                User = user
            });

            return Ok(new ProcessPaymentResponseDto
            {
                PaymentId = paymentResult.Payment.Id,
                Success = paymentResult.Payment.Success
            });
        }

        [HttpGet("{paymentId}")]
        public async Task<ActionResult<PaymentDetailsDto>> GetPaymentById([FromRoute, Required] string paymentId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var payment = await _paymentsService.GetPaymentByIdAsync(paymentId);

            if (payment == null) return NotFound();

            if (payment.UserId != user.Id) return Forbid();

            var paymentDto = _detailsDtoFactory.CreatePaymentDetailsDto(payment);
            
            return Ok(paymentDto);
        }
    }
}