using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.API.Services.Interfaces;

namespace PaymentGateway.API.Services
{
    public class SigningKeyService : ISigningKeyService
    {
        private const string TokenKey = "TokenKey";

        private readonly IConfiguration _configuration;
        private readonly ILogger<SigningKeyService> _logger;

        public SigningKeyService(IConfiguration configuration, ILogger<SigningKeyService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public SymmetricSecurityKey GetSigningKey()
        {
            var tokenKey = _configuration[TokenKey];

            if (!string.IsNullOrWhiteSpace(tokenKey)) return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            _logger.LogError("Invalid TokenKey or TokenKey not specified in configuration");
            throw new KeyNotFoundException();
        }
    }
}