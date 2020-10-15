using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Data;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Data.Repositories;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Services;
using PaymentGateway.Helpers;

namespace PaymentGateway.Extensions
{
    public static class AppServiceCollectionExtensions
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
                    options.UseSqlite(configuration.GetConnectionString("PaymentGateway"),
                        builder => { builder.MigrationsAssembly("PaymentGateway.Data"); });
                }
            });

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ISigningKeyService, SigningKeyService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IPaymentsRepository, PaymentsRepository>();
            services.AddTransient<IPaymentsService, PaymentsService>();
            services.AddTransient<IPaymentProcessor, PaymentProcessor>();
            services.AddTransient<IPaymentDetailsDtoFactory, PaymentDetailsDtoFactory>();

            if (environment.IsDevelopment())
            {
                services.AddTransient<IAcquiringBank, FakeAcquiringBank>();
            }
        }
    }
}