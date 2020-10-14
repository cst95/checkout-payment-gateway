using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.Models.Entities;
using Serilog;

namespace PaymentGateway.Extensions
{
    public static class DatabaseExtensions
    {
        private static readonly ILogger Log = Serilog.Log.ForContext(typeof(DatabaseExtensions));
        
        /// <summary>
        /// Runs outstanding database migrations to ensure the database is setup in startup.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void CreateDevelopmentDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<PaymentGatewayContext>();

            context.Database.Migrate();
        }

        /// <summary>
        /// Setup a basic test user for testing authentication functionality.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public static void CreateTestUser(this IApplicationBuilder app, UserManager<User> userManager)
        {
            const string testUserName = "test";
            const string testPassword = "Password123!";

            if (userManager.FindByNameAsync(testUserName).Result != null)
            {
                Log.Information("Test user already exists.");
                return;
            }
            
            var identityResult = userManager.CreateAsync(new User
            {
                UserName = testUserName
            }, testPassword).Result;

            if (identityResult.Succeeded)
            {
                Log.Information("Test user successfully created.");
            }
            else
            {
                Log.Warning("Test user creation failed.");
            }
        }
    }
}