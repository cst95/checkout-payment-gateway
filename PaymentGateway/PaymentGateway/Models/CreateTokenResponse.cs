using System;

namespace PaymentGateway.API.Models
{
    public class CreateTokenResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
    }
}