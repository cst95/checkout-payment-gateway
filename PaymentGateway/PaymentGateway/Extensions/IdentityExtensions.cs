using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.API.Data;
using PaymentGateway.API.Models;
using PaymentGateway.API.Services;

namespace PaymentGateway.API.Extensions
{
    public static class IdentityExtensions
    {
        public static string TokenKey = "TokenKey";
        
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
            // TODO: Resolve ISigningKeyService here to get the issuer signing key.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[TokenKey]))
                    };
                });

            services.AddAuthorization();
        }
    }
}