using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Models.Entities;
using PaymentGateway.Services;
using Xunit;

namespace PaymentGateway.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly AccountService _accountService;
        
        public AccountServiceTests()
        {
            var store = new Mock<IUserStore<User>>();
            var tokenServiceMock = new Mock<ITokenService>();
            var loggerMock = new Mock<ILogger<AccountService>>();

            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            tokenServiceMock.Setup(x => x.CreateJsonWebToken(It.IsAny<User>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new CreateTokenResponse());
            
            _accountService = new AccountService(_userManagerMock.Object, tokenServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async void AttemptLoginAsync_WithCorrectUsernameAndPassword_ReturnsSuccessfulLogin()
        {
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            
            var result = await _accountService.AttemptLoginAsync("username", "password");
            
            Assert.True(result.Success);
        }
        
        [Fact]
        public async void AttemptLoginAsync_WithIncorrectPassword_ReturnsFailedLogin()
        {
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            
            var result = await _accountService.AttemptLoginAsync("username", "password");
            
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(null, "password")]
        [InlineData("username", null)]
        [InlineData(null, null)]
        public async void AttemptLoginAsync_WithNullParameters_ReturnsFailedLogin(string username, string password)
        {
            var result = await _accountService.AttemptLoginAsync(username, password);
            
            Assert.False(result.Success);
            Assert.Null(result.Token);
            Assert.Null(result.Expires);
        }
        
        [Fact]
        public async void AttemptLoginAsync_WithNonExistentUser_ReturnsFailedLogin()
        {
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            
            var result = await _accountService.AttemptLoginAsync("username", "password");
            
            Assert.False(result.Success);
        }
    }
}