using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Models.DTOs;
using Xunit;

namespace PaymentGateway.Tests.API.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly AccountController _accountController;
        
        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _accountController = new AccountController(_accountServiceMock.Object);
        }
        
        [Fact]
        public async void Login_WithInvalidUsernameOrPassword_ReturnsUnauthorizedResponse()
        {
            var request = new LoginRequestDto();
            var mockResponse = new LoginResponse();
            
            _accountServiceMock.Setup(x => x.AttemptLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            var result = await _accountController.Login(request);
            
            var actionResult = Assert.IsType<ActionResult<LoginResponseDto>>(result);
            Assert.IsType<UnauthorizedResult>(actionResult.Result);
        }
        
        [Fact]
        public async void Login_WithValidUsernameAndPassword_ReturnsOkResponseWithTokenInformation()
        {
            var now = DateTime.UtcNow;
            var request = new LoginRequestDto
            {
                Username = "test",
                Password = "password"
            };
            
            var mockResponse = new LoginResponse
            {
                Success = true,
                Token = "token",
                Expires = now
            };
            
            _accountServiceMock.Setup(x => x.AttemptLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            var result = await _accountController.Login(request);

            var actionResult = Assert.IsType<ActionResult<LoginResponseDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<LoginResponseDto>(okResult.Value);
            Assert.NotNull(returnValue.Token);
            Assert.Equal(now, returnValue.Expires);
        }
    }
}