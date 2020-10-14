using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Models.Entities;

namespace PaymentGateway.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly ISigningKeyService _signingKeyService;

        public TokenService(ILogger<TokenService> logger, ISigningKeyService signingKeyService)
        {
            _logger = logger;
            _signingKeyService = signingKeyService;
        }

        public CreateTokenResponse CreateJsonWebToken(User user, DateTime currentUtcTime, int hoursUntilExpiry = 12)
        {
            if (user == null) return new CreateTokenResponse();

            var userClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };
            
            var signingKey = _signingKeyService.GetSigningKey();
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
            
            _logger.LogInformation("JSON Web Token Created for {UserName}. Value: {Token}", user.UserName, jwt);

            return new CreateTokenResponse
            {
                Token = jwt,
                Expires = expiresDate,
                Success = true
            };
        }
    }
}