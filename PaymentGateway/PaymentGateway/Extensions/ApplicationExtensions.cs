using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.API.Data;
using PaymentGateway.API.Interfaces;
using PaymentGateway.API.Services;

namespace PaymentGateway.API.Extensions
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Add the core services to the DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static void AddApplicationServices(this IServiceCollection services,
            IWebHostEnvironment environment, IConfiguration configuration)
        {
            services.AddDbContext<PaymentGatewayContext>(options =>
            {
                if (environment.IsDevelopment())
                {
                    options.UseSqlite(configuration.GetConnectionString("PaymentGateway"));
                }
            });
            
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ISigningKeyService, SigningKeyService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IPaymentsRepository, PaymentsRepository>();
        }
    }
}