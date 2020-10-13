using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.API.Models;
using PaymentGateway.API.Services;
using Xunit;

namespace PaymentGateway.Tests.Services
{
    public class TokenServiceTests
    {
        private const string TokenKey = "TokenKey";

        private readonly TokenService _tokenService;
        private readonly DateTime _now;
        private readonly User _validUser;

        private readonly Mock<IConfiguration> _configurationMock;

        public TokenServiceTests()
        {
            var loggerMock = new Mock<ILogger<TokenService>>();

            _now = DateTime.UtcNow;
            _validUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser"
            };

            _configurationMock = new Mock<IConfiguration>();
            _tokenService = new TokenService(_configurationMock.Object, loggerMock.Object);

            _configurationMock.Setup(x => x[TokenKey]).Returns("A signing key which is sufficiently long.");
        }

        [Fact]
        public void CreateToken_WithValidParameters_ReturnsTokenAndSuccess()
        {
            var result = _tokenService.CreateJsonWebToken(_validUser, _now);
            
            Assert.True(result.Success);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.Expires);
        }
        
        [Fact]
        public void CreateToken_WithNullUserParameter_ReturnsSuccessFalse()
        {
            var result = _tokenService.CreateJsonWebToken(null, _now);

            Assert.False(result.Success);
            Assert.Null(result.Expires);
            Assert.Null(result.Token);
        }

        [Theory]
        [InlineData((string) null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateToken_WithNoTokenSetInConfig_ThrowsKeyNotFoundException(string tokenKey)
        {
            _configurationMock.Setup(x => x[TokenKey]).Returns(tokenKey);

            Action act = () => _tokenService.CreateJsonWebToken(_validUser, _now);

            Assert.Throws<KeyNotFoundException>(act);
        }

        [Fact]
        public void CreateToken_WithTooShortKeyInConfig_ThrowsArgumentOutOfRangeException()
        {
            _configurationMock.Setup(x => x[TokenKey]).Returns("shortkey");
            
            Action act = () => _tokenService.CreateJsonWebToken(_validUser, _now);

            Assert.Throws<ArgumentOutOfRangeException>(act);

        }

        [Theory]
        [MemberData(nameof(Dates))]
        public void CreateToken_WithValidParameters_CreatesTokenWithCorrectExpiresDate(int hoursUntilExpiry,
            DateTime dateTime,
            DateTime expected)
        {
            var result = _tokenService.CreateJsonWebToken(_validUser, dateTime, hoursUntilExpiry);
            Assert.Equal(result.Expires, expected);
        }

        public static IEnumerable<object[]> Dates =>
            new List<object[]>
            {
                new object[]
                {
                    12,
                    new DateTime(2030, 1, 1, 0, 0, 0),
                    new DateTime(2030, 1, 1, 12, 0, 0),
                },
                new object[]
                {
                    5,
                    new DateTime(2030, 1, 1, 0, 0, 0),
                    new DateTime(2030, 1, 1, 5, 0, 0),
                },
                new object[]
                {
                    48,
                    new DateTime(2030, 1, 1, 0, 0, 0),
                    new DateTime(2030, 1, 3, 0, 0, 0),
                },
            };
    }
}