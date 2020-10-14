using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Models;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Data
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly PaymentGatewayContext _dbContext;
        private readonly ILogger<PaymentsRepository> _logger;

        public PaymentsRepository(PaymentGatewayContext dbContext, ILogger<PaymentsRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<SavePaymentResult> SaveProcessedPaymentAsync(IProcessedPayment processedPayment)
        {
            var payment = MapPayment(processedPayment);
            
            await _dbContext.Payments.AddAsync(payment);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError("Failed to save payment to the database: {PaymentId}. {Exception}", payment.Id,
                    exception);

                return new SavePaymentResult
                {
                    Success = false,
                    Payment = payment
                };
            }

            _logger.LogInformation("Payment successfully saved to the database: {PaymentId}", payment.Id);

            return new SavePaymentResult
            {
                Success = true,
                Payment = payment
            };
        }

        private Payment MapPayment(IProcessedPayment processedPayment) => new Payment
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