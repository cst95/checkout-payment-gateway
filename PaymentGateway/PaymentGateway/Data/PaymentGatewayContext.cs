using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Data
{
    public class PaymentGatewayContext : IdentityDbContext<User>
    {
        public PaymentGatewayContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}