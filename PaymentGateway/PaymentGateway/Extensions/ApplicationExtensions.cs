﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.API.Data;
using PaymentGateway.API.Services;
using PaymentGateway.API.Services.Interfaces;

namespace PaymentGateway.API.Extensions
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Add the core services to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static void AddApplicationServices(this IServiceCollection serviceCollection,
            IWebHostEnvironment environment, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<PaymentGatewayContext>(options =>
            {
                if (environment.IsDevelopment())
                {
                    options.UseSqlite(configuration.GetConnectionString("PaymentGateway"));
                }
            });
            
            serviceCollection.AddTransient<ITokenService, TokenService>();
            serviceCollection.AddTransient<ISigningKeyService, SigningKeyService>();
            serviceCollection.AddTransient<IAccountService, AccountService>();
        }
    }
}