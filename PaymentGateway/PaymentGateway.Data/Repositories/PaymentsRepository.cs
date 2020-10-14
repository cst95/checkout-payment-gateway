﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Data.Models;
using PaymentGateway.Data.Models.Entities;

namespace PaymentGateway.Data.Repositories
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
    }
}