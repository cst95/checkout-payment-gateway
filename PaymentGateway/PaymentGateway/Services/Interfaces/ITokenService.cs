using System;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Services.Interfaces
{
    public interface ITokenService
    {
        CreateTokenResponse CreateJsonWebToken(User user, DateTime currentUtcTime, int hoursUntilExpiry);
    }
}