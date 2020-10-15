using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Models.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login and obtain your Bearer token")]
        [Produces("application/json"), Consumes("application/json")]
        [SwaggerResponse(200, "Login successful", typeof(LoginResponseDto))]
        [SwaggerResponse(401, "Login unsuccessful")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
        {
            var loginResult = await _accountService.AttemptLoginAsync(loginRequest.Username, loginRequest.Password);

            if (!loginResult.Success) return Unauthorized();

            return Ok(new LoginResponseDto
            {
                Token = loginResult.Token,
                Expires = loginResult.Expires.GetValueOrDefault()
            });
        }
    }
}