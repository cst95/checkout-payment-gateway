using System;

namespace PaymentGateway.API.Models.DTOs
{
    public class LoginResponseDto
    {
        /// <summary>
        /// The Json Web Token created for the logged in user.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// DateTime at which the token expires.
        /// </summary>
        public DateTime Expires { get; set; }
    }
}