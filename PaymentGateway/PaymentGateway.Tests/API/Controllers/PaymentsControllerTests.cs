using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Models;
using PaymentGateway.Models.DTOs;
using Xunit;

namespace PaymentGateway.Tests.API.Controllers
{
    public class PaymentsControllerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IPaymentProcessor> _paymentProcessorMock;
        private readonly PaymentsController _paymentsController;

        public PaymentsControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _paymentProcessorMock = new Mock<IPaymentProcessor>();

            _paymentsController = new PaymentsController(_userManagerMock.Object, _paymentProcessorMock.Object);
        }

        [Fact]
        public async void ProcessPayment_WithValidPaymentRequest_ReturnsOkResponseWithCorrectContent()
        {
            const bool success = true;
            const string id = "10";
            
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User.Identity.Name).Returns("username");
            _paymentsController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new User());

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
    }
}