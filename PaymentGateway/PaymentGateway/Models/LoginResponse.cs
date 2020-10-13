using System;

namespace PaymentGateway.API.Models
{
    public class LoginResponse
    {
        /// <summary>
        /// Indicates whether the login attempt was successful.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// The logged in user's Json Web Token.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The Expiry DateTime of the Json Web Token.
        /// </summary>
        public DateTime? Expires { get; set; }
    }
}