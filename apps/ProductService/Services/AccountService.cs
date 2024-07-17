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
            _logger.LogInformation("Register method called with Email: {Email}, Username: {Username}, Role: {Role}", request.Email, request.Username, request.Role);

            if (string.IsNullOrEmpty(request.Role))
            {
                _logger.LogWarning("Role is required for registration.");
                return new RegisterReply
                {
                    Success = false,
                    Message = "Role is required."
                };
            }

            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Email {Email} is already registered.", request.Email);
                    return new RegisterReply
                    {
                        Success = false,
                        Message = "Email is already registered."
                    };
                }

                var user = new User
                {
                    UserName = request.Username,
                    Email = request.Email,
                    Role = request.Role
                };

                _logger.LogInformation("Attempting to create user {Username}", request.Username);

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Username} registered successfully", request.Username);
                    return new RegisterReply
                    {
                        Success = true,
                        Message = "User registered successfully."
                    };
                }
                else
                {
                    _logger.LogWarning("User registration failed for {Username}: {Errors}", request.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
                    return new RegisterReply
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user {Username}", request.Username);
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

                public override async Task<GetUserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetUser method called with Email: {Email}", request.Email);
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    return new GetUserReply
                    {
                        Success = true,
                        Message = "User found.",
                        Username = user.UserName,
                        Role = user.Role
                    };
                }

                _logger.LogWarning("User not found.");
                return new GetUserReply
                {
                    Success = false,
                    Message = "User not found."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user.");
                throw new RpcException(new Status(StatusCode.Unknown, "Internal server error"), ex.Message);
            }
        }

        public override async Task<DeleteUserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("DeleteUser method called with Email: {Email}", request.Email);
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User deleted successfully.");
                        return new DeleteUserReply
                        {
                            Success = true,
                            Message = "User deleted successfully."
                        };
                    }
                    else
                    {
                        _logger.LogWarning("User deletion failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                        return new DeleteUserReply
                        {
                            Success = false,
                            Message = string.Join(", ", result.Errors.Select(e => e.Description))
                        };
                    }
                }

                _logger.LogWarning("User not found.");
                return new DeleteUserReply
                {
                    Success = false,
                    Message = "User not found."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user.");
                throw new RpcException(new Status(StatusCode.Unknown, "Internal server error"), ex.Message);
            }
        }

         public override async Task<UpdateUserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("UpdateUser method called with Email: {Email}, NewUsername: {NewUsername}, NewRole: {NewRole}", request.Email, request.NewUsername, request.NewRole);
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogWarning("User not found.");
                    return new UpdateUserReply
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                user.UserName = request.NewUsername ?? user.UserName;
                user.Role = request.NewRole ?? user.Role;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User updated successfully.");
                    return new UpdateUserReply
                    {
                        Success = true,
                        Message = "User updated successfully."
                    };
                }
                else
                {
                    _logger.LogWarning("User update failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    return new UpdateUserReply
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user.");
                throw new RpcException(new Status(StatusCode.Unknown, "Internal server error"), ex.Message);
            }
        }

        public override async Task<ChangePasswordReply> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
        {
            _logger.LogInformation("ChangePassword method called with Email: {Email}", request.Email);
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogWarning("User not found.");
                    return new ChangePasswordReply
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Password changed successfully.");
                    return new ChangePasswordReply
                    {
                        Success = true,
                        Message = "Password changed successfully."
                    };
                }
                else
                {
                    _logger.LogWarning("Password change failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    return new ChangePasswordReply
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password.");
                throw new RpcException(new Status(StatusCode.Unknown, "Internal server error"), ex.Message);
            }
        }
    }
}
