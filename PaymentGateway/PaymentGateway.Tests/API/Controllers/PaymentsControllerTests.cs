using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Helpers;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Models.DTOs;
using Xunit;

namespace PaymentGateway.Tests.API.Controllers
{
    public class PaymentsControllerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IPaymentProcessor> _paymentProcessorMock;
        private readonly Mock<IPaymentsService> _paymentsServiceMock;
        private readonly Mock<IPaymentDetailsDtoFactory> _paymentDetailsDtoFactoryMock;
        private readonly PaymentsController _paymentsController;

        public PaymentsControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock =
                new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            
            _paymentProcessorMock = new Mock<IPaymentProcessor>();
            _paymentsServiceMock = new Mock<IPaymentsService>();
            _paymentDetailsDtoFactoryMock = new Mock<IPaymentDetailsDtoFactory>();
            
            _paymentsController = new PaymentsController(_userManagerMock.Object, _paymentProcessorMock.Object,
                _paymentsServiceMock.Object, _paymentDetailsDtoFactoryMock.Object);
            
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User.Identity.Name).Returns("username");
            _paymentsController.ControllerContext = new ControllerContext
            {
                HttpContext = mockContext.Object
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new User());
        }

        [Fact]
        public async void ProcessPayment_WithValidPaymentRequest_ReturnsOkResponseWithCorrectContent()
        {
            const bool success = true;
            const string id = "10";

            _paymentProcessorMock.Setup(x => x.ProcessAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(
                new ProcessPaymentResult
                {
                    Payment = new ProcessedPayment
                    {
                        Success = success,
                        Id = id
                    }
                });

            var result = await _paymentsController.ProcessPayment(new ProcessPaymentRequestDto());

            var actionResult = Assert.IsType<ActionResult<ProcessPaymentResponseDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<ProcessPaymentResponseDto>(okResult.Value);

            Assert.Equal(success, returnValue.Success);
            Assert.Equal(id, returnValue.PaymentId);
        }

        [Fact]
        public async void GetPaymentById_WhenUserIdDoesntEqualPaymentsUserId_ReturnsForbidResult()
        {
            var userId = Guid.NewGuid().ToString();
            var paymentUserId = Guid.NewGuid().ToString();
            
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new User
            {
                Id = userId
            });

            _paymentsServiceMock.Setup(x => x.GetPaymentByIdAsync(It.IsAny<string>())).ReturnsAsync(new PaymentDetails
            {
                UserId = paymentUserId
            });
            
            var result = await _paymentsController.GetPaymentById(It.IsAny<string>());

            var actionResult = Assert.IsType<ActionResult<PaymentDetailsDto>>(result);
            Assert.IsType<ForbidResult>(actionResult.Result);
        }
        
        [Fact]
        public async void GetPaymentById_WhenPaymentIsNull_ReturnsNotFoundResult()
        {
            _paymentsServiceMock.Setup(x => x.GetPaymentByIdAsync(It.IsAny<string>())).ReturnsAsync((PaymentDetails)null);
            
            var result = await _paymentsController.GetPaymentById(It.IsAny<string>());

            var actionResult = Assert.IsType<ActionResult<PaymentDetailsDto>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
        
        [Fact]
        public async void GetPaymentById_WhenUserIsCorrectAndPaymentExists_ReturnsOkResultWithCorrectPaymentDetails()
        {
            const string paymentId = "1234";
            const string userId = "1234";
            
            var expectedPaymentDetails = new PaymentDetails
            {
                Id = paymentId,
                UserId = userId
            };
            
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new User
            {
                Id = userId
            });
            
            _paymentsServiceMock.Setup(x => x.GetPaymentByIdAsync(paymentId)).ReturnsAsync(expectedPaymentDetails);

            _paymentDetailsDtoFactoryMock.Setup(x => x.CreatePaymentDetailsDto(It.IsAny<PaymentDetails>()))
                .Returns(
                    new PaymentDetailsDto
                    {
                        PaymentId = paymentId
                    });
            
            var result = await _paymentsController.GetPaymentById(paymentId);
            
            var actionResult = Assert.IsType<ActionResult<PaymentDetailsDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<PaymentDetailsDto>(okResult.Value);
            
            Assert.Equal(paymentId, returnValue.PaymentId);
        }
    }
}