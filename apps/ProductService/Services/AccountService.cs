using Grpc.Core;
using GrpcServices.AccountService;
using GrpcServices.Models;  // 确保引用了Models命名空间
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServices.Services
{
    public class AccountService : Account.AccountBase
    {
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<User> _userManager;

        public AccountService(ILogger<AccountService> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public override async Task<RegisterReply> Register(RegisterRequest request, ServerCallContext context)
        {
            var user = new User
            {
                UserName = request.Username,
                Email = request.Email
            };

            string hashedPassword = PasswordHasher.HashPassword(request.Password);
            var result = await _userManager.CreateAsync(user, hashedPassword);

            if (result.Succeeded)
            {
                return new RegisterReply
                {
                    Success = true,
                    Message = "User registered successfully."
                };
            }
            else
            {
                return new RegisterReply
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }
        }

        public override async Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user != null)
            {
                string hashedPassword = PasswordHasher.HashPassword(request.Password);
                if (await _userManager.CheckPasswordAsync(user, hashedPassword))
                {
                    return new LoginReply
                    {
                        Success = true,
                        Message = "User logged in successfully."
                    };
                }
            }

            return new LoginReply
            {
                Success = false,
                Message = "Invalid username or password."
            };
        }

        public override Task<RememberPasswordReply> RememberPassword(RememberPasswordRequest request, ServerCallContext context)
        {
            // 在这里实现您的记住密码逻辑
            return Task.FromResult(new RememberPasswordReply
            {
                Success = true,
                Message = "Password remembered."
            });
        }

        public override Task<ResetPasswordReply> ResetPassword(ResetPasswordRequest request, ServerCallContext context)
        {
            // 在这里实现您的重设密码逻辑
            return Task.FromResult(new ResetPasswordReply
            {
                Success = true,
                Message = "Password reset successfully."
            });
        }
    }
}
