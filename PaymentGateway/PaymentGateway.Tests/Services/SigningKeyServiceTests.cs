using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Services;
using Xunit;

namespace PaymentGateway.Tests.Services
{
    public class SigningKeyServiceTests
    {
        private const string TokenKey = "TokenKey";

        private readonly Mock<IConfiguration> _configurationMock;
        private readonly SigningKeyService _signingKeyService;

        public SigningKeyServiceTests()
        {
            var loggerMock = new Mock<ILogger<SigningKeyService>>();
            _configurationMock = new Mock<IConfiguration>();
            _signingKeyService = new SigningKeyService(_configurationMock.Object, loggerMock.Object);
        }

        [Fact]
        public void GetSigningKey_WithValidTokenSetInConfig_ReturnsSigningKey()
        {
            _configurationMock.Setup(x => x[TokenKey]).Returns("My valid signing key.");

            var result = _signingKeyService.GetSigningKey();

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData((string) null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetSigningKey_WithInvalidTokenSetInConfig_ThrowsKeyNotFoundException(string tokenKey)
        {
            _configurationMock.Setup(x => x[TokenKey]).Returns(tokenKey);

            Action act = () => _signingKeyService.GetSigningKey();

            Assert.Throws<KeyNotFoundException>(act);
        }
    }
}