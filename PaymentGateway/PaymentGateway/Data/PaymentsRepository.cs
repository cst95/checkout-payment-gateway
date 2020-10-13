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

        public async Task<SavePaymentResult> SavePaymentAsync(SavePaymentRequest paymentRequest)
        {
            var payment = GetPayment(paymentRequest);
            
            await _dbContext.Payments.AddAsync(payment);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                _logger.LogWarning("Failed to save payment to the database: {Payment}. {Exception}", paymentRequest, exception);
            }
            
            _logger.LogDebug("Payment successfully saved to the database: {Payment}", paymentRequest);

            return new SavePaymentResult
            {
                Payment = payment
            };
        }
        
        private Payment GetPayment(SavePaymentRequest request) => new Payment
        {
            Card = new Card
            {
                Id = request.PaymentRequest.CardNumber,
                ExpiryDate = request.PaymentRequest.CardExpiryDate,
                Cvv = request.PaymentRequest.CardCvv
            },
            UserId = request.PaymentRequest.User.Id,
            User = request.PaymentRequest.User,
            DateTime = DateTime.UtcNow,
            Id = request.AcquiringBankResponse.Id,
            Success = request.AcquiringBankResponse.Success,
            Amount = request.PaymentRequest.Amount,
            CardId = request.PaymentRequest.CardNumber,
            Currency = request.PaymentRequest.Currency
        };
    }
}