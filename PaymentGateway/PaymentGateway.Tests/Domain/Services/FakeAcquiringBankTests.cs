using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Services;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.Tests.Domain.Services
{
    public class FakeAcquiringBankTests
    {
        private readonly FakeAcquiringBank _fakeAcquiringBank;

        public FakeAcquiringBankTests()
        {
            var loggerMock = new Mock<ILogger<FakeAcquiringBank>>();
            _fakeAcquiringBank = new FakeAcquiringBank(loggerMock.Object);
        }

        [Fact]
        public async void CreateRequestAsync_WithNullParameter_ThrowsArgumentNullException()
        {
            Func<Task> act = () => _fakeAcquiringBank.CreateRequestAsync(null);

            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async void CreateRequestAsync_WithValidParameter_ReturnsValidRequest()
        {
            var request = new PaymentRequest
            {
                CardNumber = "1111",
                CardCvv = 111,
                Currency = Currency.GBP,
                CardExpiryMonth = 2,
                CardExpiryYear = 2020,
                Amount = 12
            };

            var result = await _fakeAcquiringBank.CreateRequestAsync(request);
            
            Assert.Equal(request.CardNumber, result.CardNumber);
            Assert.Equal(request.CardCvv, result.CardCvv);
            Assert.Equal(request.Currency, result.Currency);
            Assert.Equal(request.CardExpiryMonth, result.CardExpiryMonth);
            Assert.Equal(request.CardExpiryYear, result.CardExpiryYear);
            Assert.Equal(request.Amount, result.Amount);
        }
        
        [Fact]
        public async void ProcessPaymentAsync_WithNullParameter_ThrowsArgumentNullException()
        {
            Func<Task> act = () => _fakeAcquiringBank.ProcessPaymentAsync(null);

            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async void ProcessPaymentAsync_RequestAmountGreaterThan500_ReturnsSuccessFalse()
        {
            var request = new FakeAcquiringBankRequest
            {
                CardNumber = "1111",
                CardCvv = 111,
                Currency = Currency.GBP,
                CardExpiryMonth = 2,
                CardExpiryYear = 2020,
                Amount = 501
            };
            
            var result = await _fakeAcquiringBank.ProcessPaymentAsync(request);
            
            Assert.False(result.Success);
            Assert.NotNull(result.PaymentId);
        }
        
        [Fact]
        public async void ProcessPaymentAsync_RequestAmountLessThan500_ReturnsSuccessTrue()
        {
            var request = new FakeAcquiringBankRequest
            {
                CardNumber = "1111",
                CardCvv = 111,
                Currency = Currency.GBP,
                CardExpiryMonth = 2,
                CardExpiryYear = 2020,
                Amount = 200
            };
            
            var result = await _fakeAcquiringBank.ProcessPaymentAsync(request);
            
            Assert.True(result.Success);
            Assert.NotNull(result.PaymentId);
        }
    }
}