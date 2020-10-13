using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Models.DTOs;
using PaymentGateway.API.Services.Interfaces;

namespace PaymentGateway.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
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