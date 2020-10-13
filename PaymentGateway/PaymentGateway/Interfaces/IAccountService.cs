using System.Threading.Tasks;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Attempts to login the user with the given username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Login response indicating the outcome of the login attempt.</returns>
        Task<LoginResponse> AttemptLoginAsync(string username, string password);
    }
}