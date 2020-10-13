using System;

namespace PaymentGateway.API.Models
{
    public class CreateTokenResponse
    {
        /// <summary>
        /// Indicates whether the creation of the token was a success.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// The Json Web Token.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// DateTime at which the token expires.
        /// </summary>
        public DateTime? Expires { get; set; }
    }
}