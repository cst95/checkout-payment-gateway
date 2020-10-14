using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Services
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

            var saveResult = await _paymentsRepository.SaveProcessedPaymentAsync(processedPayment);

            return new ProcessPaymentResult
            {
                Payment = saveResult.Payment
            };
        }
    }
}