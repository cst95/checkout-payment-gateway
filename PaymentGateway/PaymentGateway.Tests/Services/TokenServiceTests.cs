using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PaymentGateway.Interfaces;
using PaymentGateway.Models.Entities;
using PaymentGateway.Services;
using Xunit;

namespace PaymentGateway.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly DateTime _now;
        private readonly User _validUser;

        public TokenServiceTests()
        {
            _now = DateTime.UtcNow;
            _validUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Test User"
            };

            var loggerMock = new Mock<ILogger<TokenService>>();
            var signingKeyServiceMock = new Mock<ISigningKeyService>();

            signingKeyServiceMock.Setup(s => s.GetSigningKey())
                .Returns(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A sufficiently long signing key.")));

            _tokenService = new TokenService(loggerMock.Object, signingKeyServiceMock.Object);
        }

        [Fact]
        public void CreateToken_WithValidParameters_ReturnsTokenAndSuccess()
        {
            var result = _tokenService.CreateJsonWebToken(_validUser, _now);

            Assert.True(result.Success);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.Expires);
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

        [Fact]
        public void CreateToken_WithNullUserParameter_ReturnsSuccessFalse()
        {
            var result = _tokenService.CreateJsonWebToken(null, _now);

            Assert.False(result.Success);
            Assert.Null(result.Expires);
            Assert.Null(result.Token);
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