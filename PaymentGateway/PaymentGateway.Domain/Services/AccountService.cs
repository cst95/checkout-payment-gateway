﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PaymentGateway.Data.Models.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<User> userManager, ITokenService tokenService, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }
        
        public async Task<LoginResponse> AttemptLoginAsync(string username, string password)
        {
            if (username == null || password == null) return new LoginResponse();

            var user = await _userManager.FindByNameAsync(username.ToLower());
            
            if (user == null) return new LoginResponse();

            var loginSuccess = await _userManager.CheckPasswordAsync(user, password);

            if (!loginSuccess)
            {
                _logger.LogWarning("{UserName} has attempted to login with an incorrect password.", user.UserName);
                return new LoginResponse();
            }

            _logger.LogInformation("{UserName} has successfully logged in.", user.UserName);
            
            var tokenResponse = _tokenService.CreateJsonWebToken(user, DateTime.UtcNow);

            return new LoginResponse
            {
                Success = true,
                Expires = tokenResponse.Expires,
                Token = tokenResponse.Token
            };
        }
    }
}