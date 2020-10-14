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
        private readonly ILogger<PaymentsService> _logger;
        private readonly IPaymentsService _paymentsService;

        public PaymentProcessor(IPaymentsRepository paymentsRepository, IAcquiringBank acquiringBank,
            ILogger<PaymentsService> logger, IPaymentsService paymentsService)
        {
            _paymentsRepository = paymentsRepository;
            _acquiringBank = acquiringBank;
            _logger = logger;
            _paymentsService = paymentsService;
        }

        public async Task<ProcessPaymentResult> ProcessAsync(IPaymentRequest paymentRequest)
        {
            var unprocessedPayment = _paymentsService.CreateUnprocessedPayment(paymentRequest);

            // TODO: Implement some decision here as to which acquiring bank implementation is used. Perhaps use factory pattern.
            IAcquiringBankResponse acquiringBankResponse = null;

            try
            {
                var acquiringBankRequest = await _acquiringBank.CreateRequestAsync(unprocessedPayment);
                acquiringBankResponse = await _acquiringBank.ProcessPaymentAsync(acquiringBankRequest);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Exception occured whilst the acquiring bank was processing payment {paymentId}. {Exception}",
                    unprocessedPayment.Id, exception);
            }

            var processedPayment = _paymentsService.CreateProcessedPayment(unprocessedPayment, acquiringBankResponse);
            
            _logger.LogInformation("Payment {paymentId} has successfully been processed.", processedPayment.Id);

            await _paymentsRepository.SavePaymentAsync(CreatePayment(processedPayment));

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
            DateTime = processedPayment.DateTime,
            Success = processedPayment.Success
        };
    }
}