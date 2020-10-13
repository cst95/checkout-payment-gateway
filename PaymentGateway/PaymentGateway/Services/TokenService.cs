using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.API.Models;
using PaymentGateway.API.Services.Interfaces;

namespace PaymentGateway.API.Services
{
    public class TokenService : ITokenService
    {
        private const string TokenKey = "TokenKey";
        private const int HoursUntilExpiry = 12;

        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public CreateTokenResponse CreateJsonWebToken(User user, DateTime currentUtcTime, int hoursUntilExpiry = HoursUntilExpiry)
        {
            if (user == null) return new CreateTokenResponse();

            var userClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };
            
            var signingKey = GetSigningKey();
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature);
            
            var expiresDate = currentUtcTime.AddHours(hoursUntilExpiry);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = expiresDate,
                SigningCredentials = signingCredentials
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new CreateTokenResponse
            {
                Token = jwt,
                Expires = expiresDate,
                Success = true
            };
        }

        private SymmetricSecurityKey GetSigningKey()
        {
            var tokenKey = _configuration[TokenKey];

            if (string.IsNullOrWhiteSpace(tokenKey))
            {
                _logger.LogError("Invalid TokenKey or TokenKey not specified in configuration");
                throw new KeyNotFoundException();
            }

            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
                return securityKey;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                _logger.LogError("TokenKey is not long enough. {Exception}", exception);
                throw;
            }
        }
    }
}