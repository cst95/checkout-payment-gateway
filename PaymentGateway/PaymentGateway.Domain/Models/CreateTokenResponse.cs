using System;

namespace PaymentGateway.Domain.Models
{
    public class CreateTokenResponse
    {
        /// <summary>
        /// Indicates whether the creation of the token was a success.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// The created JSON Web Token.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// DateTime at which the token expires.
        /// </summary>
        public DateTime? Expires { get; set; }
    }
}