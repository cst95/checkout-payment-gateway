﻿using System;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Services;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.Tests.Domain.Services
{
    public class PaymentsServiceTests
    {
        private readonly PaymentsService _paymentsService;
        
        public PaymentsServiceTests()
        {
            _paymentsService = new PaymentsService();
        }
        
        [Fact]
        public void CreateUnprocessedPayment_WithValidRequest_ReturnsNonNullId()
        {
            var request = new PaymentRequest
            {
                User = new User()
            };

            var result = _paymentsService.CreateUnprocessedPayment(request);
            
            Assert.NotNull(result.Id);
        }

        [Fact]
        public void CreateUnprocessedPayment_WithNullArgument_ThrowsArgumentNullException()
        {
            PaymentRequest request = null;

            Action act = () => _paymentsService.CreateUnprocessedPayment(request);

            Assert.Throws<ArgumentNullException>(act);
        }
        
        [Fact]
        public void CreateUnprocessedPayment_WithNullUser_ThrowsArgumentException()
        {
            var request = new PaymentRequest();

            Action act = () => _paymentsService.CreateUnprocessedPayment(request);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void CreateUnprocessedPayment_WithValidRequest_CorrectlyMapsValues()
        {
            var request = new PaymentRequest
            {
                CardNumber = "1111",
                CardCvv = 111,
                Currency = Currency.GBP,
                CardExpiryMonth = 2,
                CardExpiryYear = 2020,
                Amount = 12,
                User = new User()
            };

            var result = _paymentsService.CreateUnprocessedPayment(request);
            
            Assert.Equal(request.CardNumber, result.CardNumber);
            Assert.Equal(request.CardCvv, result.CardCvv);
            Assert.Equal(request.Currency, result.Currency);
            Assert.Equal(request.CardExpiryMonth, result.CardExpiryMonth);
            Assert.Equal(request.CardExpiryYear, result.CardExpiryYear);
            Assert.Equal(request.Amount, result.Amount);
            Assert.True(request.User.Equals(result.User));
        }

        [Fact]
        public void CreateProcessedPayment_WithNullAcquiringBankResponse_ReturnsUnsuccessfulResponse()
        {
            var unprocessedPayment = new UnprocessedPayment();
            IAcquiringBankResponse acquiringBankResponse = null;

            var result = _paymentsService.CreateProcessedPayment(unprocessedPayment, acquiringBankResponse);
            
            Assert.False(result.Success);
            Assert.Null(result.AcquiringBankPaymentId);
        }
        
        [Fact]
        public void CreateProcessedPayment_WithValidParameters_CorrectlyMapsValues()
        {
            var unprocessedPayment = new UnprocessedPayment
            {
                CardNumber = "1111",
                CardCvv = 111,
                Currency = Currency.GBP,
                CardExpiryMonth = 2,
                CardExpiryYear = 2020,
                Amount = 12,
                User = new User(),
                UserId = "1",
                DateTime = DateTime.Now,
                Id = "1"
            };
            
            var acquiringBankResponse = new FakeAcquiringBankResponse
            {
                Success = true,
                PaymentId = "testid"
            };
            
            var result = _paymentsService.CreateProcessedPayment(unprocessedPayment, acquiringBankResponse);
            
            Assert.Equal(unprocessedPayment.Amount, result.Amount);
            Assert.Equal(unprocessedPayment.Currency, result.Currency);
            Assert.Equal(unprocessedPayment.Id, result.Id);
            Assert.Equal(unprocessedPayment.CardCvv, result.CardCvv);
            Assert.Equal(unprocessedPayment.CardNumber, result.CardNumber);
            Assert.Equal(unprocessedPayment.DateTime, result.DateTime);
            Assert.Equal(unprocessedPayment.UserId, result.UserId);
            Assert.Equal(unprocessedPayment.CardExpiryMonth, result.CardExpiryMonth);
            Assert.Equal(unprocessedPayment.CardExpiryYear, result.CardExpiryYear);
            Assert.True(unprocessedPayment.User.Equals(result.User));
        }
        
        [Fact]
        public void CreateProcessedPayment_WithValidAcquiringBankResponse_ReturnsCorrectResponse()
        {
            var unprocessedPayment = new UnprocessedPayment();
            var acquiringBankResponse = new FakeAcquiringBankResponse
            {
                Success = true,
                PaymentId = "testid"
            };

            var result = _paymentsService.CreateProcessedPayment(unprocessedPayment, acquiringBankResponse);
            
            Assert.Equal(acquiringBankResponse.Success, result.Success);
            Assert.Equal(acquiringBankResponse.PaymentId, result.AcquiringBankPaymentId);
        }

        [Fact]
        public void CreateProcessedPayment_WithNullAcquiringBankResponse_ThrowsArgumentNullException()
        {
            UnprocessedPayment unprocessedPayment = null;
            
            Action act = () => _paymentsService.CreateProcessedPayment(unprocessedPayment, null);

            Assert.Throws<ArgumentNullException>(act);
        }
    }
}