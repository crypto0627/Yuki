using Grpc.Core;
using GrpcServices.AccountService;
using GrpcServices.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServices.Services
{
    public class AccountService : Account.AccountBase
    {
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(ILogger<AccountService> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public override async Task<RegisterReply> Register(RegisterRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Register method called with Username: {Username} and Email: {Email}", request.Username, request.Email);
            try
            {
                var user = new User
                {
                    UserName = request.Username,
                    Email = request.Email
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered successfully.");
                    return new RegisterReply
                    {
                        Success = true,
                        Message = "User registered successfully."
                    };
                }
                else
                {
                    _logger.LogWarning("User registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    return new RegisterReply
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user.");
                throw new RpcException(new Status(StatusCode.Unknown, "Internal server error"), ex.Message);
            }
        }

        public override async Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Login method called with Email: {Email}", request.Email);
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, request.Password))
                    {
                        _logger.LogInformation("User logged in successfully.");
                        return new LoginReply
                        {
                            Success = true,
                            Message = "User logged in successfully."
                        };
                    }
                }

                _logger.LogWarning("Invalid email or password.");
                return new LoginReply
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user.");
                throw new RpcException(new Status(StatusCode.Unknown, "Internal server error"), ex.Message);
            }
        }

        public override async Task<LogoutReply> Logout(LogoutRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Logout method called with Username: {Username}", request.Username);
            try
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    _logger.LogInformation("User logged out successfully.");
                    return new LogoutReply
                    {
                        Success = true,
                        Message = "User logged out successfully."
                    };
                }

                _logger.LogWarning("User not found.");
                return new LogoutReply
                {
                    Success = false,
                    Message = "User not found."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging out user.");
                throw new RpcException(new Status(StatusCode.Unknown, "Internal server error"), ex.Message);
            }
        }
    }
}
