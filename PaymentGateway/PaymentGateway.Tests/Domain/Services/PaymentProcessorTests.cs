using System;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Services;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.Tests.Domain.Services
{
    public class PaymentProcessorTests
    {
        private readonly PaymentProcessor _paymentProcessor;
        private readonly Mock<IPaymentsService> _paymentsServiceMock;
        private readonly Mock<IAcquiringBank> _acquiringBankMock;
        private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock;

        private readonly IPaymentRequest _validRequest;

        public PaymentProcessorTests()
        {
            var loggerMock = new Mock<ILogger<PaymentProcessor>>();
            _paymentsServiceMock = SetupIPaymentsServiceMock();
            _acquiringBankMock = SetupIAcquiringBankMock();
            _paymentsRepositoryMock = new Mock<IPaymentsRepository>();

            _validRequest = new PaymentRequest
            {
                CardNumber = "1111",
                CardCvv = 111,
                Currency = Currency.GBP,
                CardExpiryMonth = 2,
                CardExpiryYear = 2020,
                Amount = 12,
                User = new User()
            };

            _paymentProcessor = new PaymentProcessor(_paymentsRepositoryMock.Object, _acquiringBankMock.Object,
                loggerMock.Object, _paymentsServiceMock.Object);
        }

        [Fact]
        public async void ProcessAsync_WithValidArgumentAndNoExceptionsThrown_ReturnsCorrectResponse()
        {
            var mockResult = _paymentsServiceMock.Object.CreateProcessedPayment(It.IsAny<IUnprocessedPayment>(),
                It.IsAny<IAcquiringBankResponse>());

            var result = await _paymentProcessor.ProcessAsync(_validRequest);

            Assert.Equal(mockResult.Success, result.Payment.Success);
            Assert.Equal(mockResult.AcquiringBankPaymentId, result.Payment.AcquiringBankPaymentId);
            Assert.Equal(mockResult.Amount, result.Payment.Amount);
            Assert.Equal(mockResult.Currency, result.Payment.Currency);
            Assert.Equal(mockResult.Id, result.Payment.Id);
            Assert.True(mockResult.User.Equals(result.Payment.User));
            Assert.Equal(mockResult.CardCvv, result.Payment.CardCvv);
            Assert.Equal(mockResult.CardNumber, result.Payment.CardNumber);
            Assert.Equal(mockResult.CreatedAt, result.Payment.CreatedAt);
            Assert.Equal(mockResult.UserId, result.Payment.UserId);
            Assert.Equal(mockResult.CardExpiryMonth, result.Payment.CardExpiryMonth);
            Assert.Equal(mockResult.CardExpiryYear, result.Payment.CardExpiryYear);
        }

        [Fact]
        public async void ProcessAsync_WithValidArgument_AlwaysCallsProcessPaymentAsyncOnce()
        {
            await _paymentProcessor.ProcessAsync(_validRequest);

            _acquiringBankMock.Verify(mock => mock.ProcessPaymentAsync(It.IsAny<IAcquiringBankRequest>()), Times.Once);
        }

        [Fact]
        public async void ProcessAsync_WithValidArgumentAndNoExceptions_AlwaysCallsSavePaymentAsyncOnce()
        {
            await _paymentProcessor.ProcessAsync(_validRequest);

            _paymentsRepositoryMock.Verify(mock => mock.SavePaymentAsync(It.IsAny<Payment>()), Times.Once);
        }

        [Fact]
        public async void ProcessAsync_WhenAcquiringBankThrowsException_CallsSavePaymentAsyncOnceWithSuccessFalseAndAcquiringBankPaymentIdNull()
        {
            _acquiringBankMock.Setup(x => x.ProcessPaymentAsync(It.IsAny<IAcquiringBankRequest>()))
                .ThrowsAsync(new Exception());

            _paymentsServiceMock.Setup(x =>
                x.CreateProcessedPayment(It.IsAny<IUnprocessedPayment>(), null)).Returns(
                new ProcessedPayment
                {
                    User = new User()
                }
            );
            
            await _paymentProcessor.ProcessAsync(_validRequest);

            _paymentsRepositoryMock.Verify(
                mock => mock.SavePaymentAsync(It.Is<Payment>(p => !p.Success && p.AcquiringBankPaymentId == null)),
                Times.Once);
        }

        private Mock<IPaymentsService> SetupIPaymentsServiceMock()
        {
            var mock = new Mock<IPaymentsService>();
            
            mock.Setup(x => x.CreateUnprocessedPayment(It.IsAny<IPaymentRequest>())).Returns(
                new UnprocessedPayment
                {
                    CardNumber = "1111",
                    CardCvv = 111,
                    Currency = Currency.GBP,
                    CardExpiryMonth = 2,
                    CardExpiryYear = 2020,
                    Amount = 12,
                    User = new User(),
                    UserId = "1",
                    CreatedAt = DateTime.Now,
                    Id = "1"
                });

            mock.Setup(x =>
                x.CreateProcessedPayment(It.IsAny<IUnprocessedPayment>(), It.IsAny<IAcquiringBankResponse>())).Returns(
                new ProcessedPayment
                {
                    CardNumber = "1111",
                    CardCvv = 111,
                    Currency = Currency.GBP,
                    CardExpiryMonth = 2,
                    CardExpiryYear = 2020,
                    Amount = 12,
                    User = new User(),
                    UserId = "1",
                    Success = true,
                    CreatedAt = DateTime.Now,
                    Id = "1",
                    AcquiringBankPaymentId = "1"
                }
            );

            return mock;
        }
        
        private Mock<IAcquiringBank> SetupIAcquiringBankMock()
        {
            var mock = new Mock<IAcquiringBank>();
            
            mock.Setup(x => x.CreateRequestAsync(It.IsAny<IPaymentRequest>())).ReturnsAsync(
                new FakeAcquiringBankRequest
                {
                    CardNumber = "1111",
                    CardCvv = 111,
                    Currency = Currency.GBP,
                    CardExpiryMonth = 2,
                    CardExpiryYear = 2020,
                    Amount = 12
                });

            mock.Setup(x => x.ProcessPaymentAsync(It.IsAny<IAcquiringBankRequest>())).ReturnsAsync(
                    new FakeAcquiringBankResponse
                    {
                        Success = true,
                        PaymentId = "testid"
                    });

            return mock;
        }
    }
}