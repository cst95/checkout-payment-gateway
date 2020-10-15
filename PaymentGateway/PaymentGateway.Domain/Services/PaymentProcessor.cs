using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Services
{
    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IAcquiringBank _acquiringBank;
        private readonly ILogger<PaymentProcessor> _logger;
        private readonly IPaymentsService _paymentsService;

        public PaymentProcessor(IPaymentsRepository paymentsRepository, IAcquiringBank acquiringBank,
            ILogger<PaymentProcessor> logger, IPaymentsService paymentsService)
        {
            _paymentsRepository = paymentsRepository;
            _acquiringBank = acquiringBank;
            _logger = logger;
            _paymentsService = paymentsService;
        }

        public async Task<ProcessPaymentResult> ProcessAsync(IPaymentRequest paymentRequest)
        {
            var unprocessedPayment = _paymentsService.CreateUnprocessedPayment(paymentRequest);
            var acquiringBankRequest = await _acquiringBank.CreateRequestAsync(paymentRequest);

            IAcquiringBankResponse acquiringBankResponse = null;
            
            try
            {
                acquiringBankResponse = await _acquiringBank.ProcessPaymentAsync(acquiringBankRequest);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Exception occured whilst the acquiring bank was processing payment {paymentId}. {Exception}",
                    unprocessedPayment.Id, exception);
            }

            var processedPayment = _paymentsService.CreateProcessedPayment(unprocessedPayment, acquiringBankResponse);
            var payment = CreatePayment(processedPayment);
            
            await _paymentsRepository.SavePaymentAsync(payment);
            
            _logger.LogInformation("Payment {paymentId} has successfully been processed.", processedPayment.Id);

            return new ProcessPaymentResult
            {
                Payment = processedPayment
            };
        }

        private Payment CreatePayment(IProcessedPayment processedPayment) => new Payment
        {
            Id = processedPayment.Id,
            CardNumber = processedPayment.CardNumber,
            CardCvv = processedPayment.CardCvv,
            CardExpiryMonth = processedPayment.CardExpiryMonth,
            CardExpiryYear = processedPayment.CardExpiryYear,
            Amount = processedPayment.Amount,
            Currency = processedPayment.Currency,
            UserId = processedPayment.User.Id,
            User = processedPayment.User,
            AcquiringBankPaymentId = processedPayment.AcquiringBankPaymentId,
            CreatedAt = processedPayment.CreatedAt,
            Success = processedPayment.Success
        };
    }
}