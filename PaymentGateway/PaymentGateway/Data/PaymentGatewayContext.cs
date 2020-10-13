using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.API.Models.Entities;

namespace PaymentGateway.API.Data
{
    public class PaymentGatewayContext : IdentityDbContext<User>
    {
        public PaymentGatewayContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Payment>()
                .HasOne(p => p.Card);

            builder.Entity<Payment>()
                .HasOne(p => p.User);

            builder.Entity<Payment>()
                .HasKey(p => p.Id);

            builder.Entity<Card>()
                .HasKey(c => c.Id);
        }
    }
}