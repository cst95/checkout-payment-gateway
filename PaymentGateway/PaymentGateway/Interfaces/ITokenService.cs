﻿using System;
using PaymentGateway.Models;
using PaymentGateway.Models.Entities;

namespace PaymentGateway.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Create a Json Web Token for the given user.
        /// </summary>
        /// <param name="user">The user to create the token for.</param>
        /// <param name="currentUtcTime">The current UTC time.</param>
        /// <param name="hoursUntilExpiry">The number of hours until the token expires.</param>
        /// <returns>A response indicating if the operation was successful, with the resulting token and expiry date.</returns>
        CreateTokenResponse CreateJsonWebToken(User user, DateTime currentUtcTime, int hoursUntilExpiry = 12);
    }
}