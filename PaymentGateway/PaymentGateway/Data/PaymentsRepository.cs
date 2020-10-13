using System;
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

        public async Task<SavePaymentResult> SavePaymentAsync(Payment payment)
        {
            await _dbContext.Payments.AddAsync(payment);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                _logger.LogWarning("Failed to save payment to the database: {Payment}. {Exception}", payment, exception);
                
                return new SavePaymentResult
                {
                    Success = false,
                    Payment = payment
                };
            }
            
            _logger.LogDebug("Payment successfully saved to the database: {Payment}", payment);

            return new SavePaymentResult
            {
                Success = true,
                Payment = payment
            };
        }
    }
}