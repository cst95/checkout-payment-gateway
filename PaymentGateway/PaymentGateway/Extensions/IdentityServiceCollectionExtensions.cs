using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.Data;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Extensions
{
    public static class IdentityServiceCollectionExtensions
    {
        private const string TokenKey = "TokenKey";

        /// <summary>
        /// Add identity/user related services to the DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>()
                .AddSignInManager<SignInManager<User>>()
                .AddEntityFrameworkStores<PaymentGatewayContext>();
        }

        /// <summary>
        /// Add JWT Authentication configuration and authorization to the DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthenticationSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<ISigningKeyService>((options, signingKeyService) =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKeyService.GetSigningKey()
                    };
                });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            services.AddAuthorization();
        }
    }
}