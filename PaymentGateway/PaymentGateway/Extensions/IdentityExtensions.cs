using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.API.Data;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Extensions
{
    public static class IdentityExtensions
    {
        /// <summary>
        /// Add identity/user related services to the DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>()
                .AddSignInManager<SignInManager<User>>()
                .AddEntityFrameworkStores<PaymentGatewayContext>();

            return services;
        }
    }
}