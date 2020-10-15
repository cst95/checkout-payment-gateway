using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Data;
using PaymentGateway.Data.Models.Entities;
using Serilog;

namespace PaymentGateway.Extensions
{
    public static class DataApplicationBuilderExtensions
    {
        private static readonly ILogger Log = Serilog.Log.ForContext(typeof(DataApplicationBuilderExtensions));
        
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
        public static void CreateTestUsers(this IApplicationBuilder app, UserManager<User> userManager)
        {
            const string testUserName = "test";
            const string testUserName2 = "test2";
            const string testPassword = "Password123!";

            CreateTestUser(userManager, testUserName, testPassword);
            CreateTestUser(userManager, testUserName2, testPassword);
        }

        private static void CreateTestUser(UserManager<User> userManager, string username, string password)
        {
            if (userManager.FindByNameAsync(username).Result != null)
            {
                Log.Information("Test user with username {Username} already exists.", username);
                return;
            }
            
            var identityResult = userManager.CreateAsync(new User
            {
                UserName = username
            }, password).Result;

            if (identityResult.Succeeded)
            {
                Log.Information("Test user with username {Username} successfully created.", username);
            }
            else
            {
                Log.Warning("Test user with username {Username} creation failed.", username);
            }
        }
    }
}