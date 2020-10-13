using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.API.Models.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}