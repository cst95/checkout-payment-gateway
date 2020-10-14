using Microsoft.IdentityModel.Tokens;

namespace PaymentGateway.Domain.Interfaces
{
    public interface ISigningKeyService
    {
        /// <summary>
        /// Get the signing key created using the tokenKey specified in IConfiguration.
        /// </summary>
        /// <returns>The symmetric signing key.</returns>
        SymmetricSecurityKey GetSigningKey();
    }
}