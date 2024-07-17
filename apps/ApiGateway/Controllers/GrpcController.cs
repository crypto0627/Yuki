using Grpc.Core;
using GrpcServices.AccountService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly Account.AccountClient _accountClient;
        private readonly ILogger<AccountController> _logger;

        public AccountController(Account.AccountClient accountClient, ILogger<AccountController> logger)
        {
            _accountClient = accountClient;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            _logger.LogInformation("Calling Register gRPC method with Email: {Email}, Username: {Username}, Role: {Role}", request.Email, request.Username, request.Role);

            try
            {
                var reply = await _accountClient.RegisterAsync(request);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed with Status: {Status} and Detail: {Detail}", ex.Status.StatusCode, ex.Status.Detail);
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }

         [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            _logger.LogInformation("Calling Login gRPC method with Email: {Email}", request.Email);
            try
            {
                var reply = await _accountClient.LoginAsync(request);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed with Status: {Status} and Detail: {Detail}", ex.Status.StatusCode, ex.Status.Detail);
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }

         [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            _logger.LogInformation("Calling Logout gRPC method with Username: {Username}", request.Username);
            try
            {
                var reply = await _accountClient.LogoutAsync(request);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed with Status: {Status} and Detail: {Detail}", ex.Status.StatusCode, ex.Status.Detail);
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }

         [HttpGet("get-user")]
        public async Task<IActionResult> GetUser([FromQuery] string email)
        {
            _logger.LogInformation("Calling GetUser gRPC method with Email: {Email}", email);
            try
            {
                var request = new GetUserRequest { Email = email };
                var reply = await _accountClient.GetUserAsync(request);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed with Status: {Status} and Detail: {Detail}", ex.Status.StatusCode, ex.Status.Detail);
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser([FromQuery] string email)
        {
            _logger.LogInformation("Calling DeleteUser gRPC method with Email: {Email}", email);
            try
            {
                var request = new DeleteUserRequest { Email = email };
                var reply = await _accountClient.DeleteUserAsync(request);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed with Status: {Status} and Detail: {Detail}", ex.Status.StatusCode, ex.Status.Detail);
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            _logger.LogInformation("Calling UpdateUser gRPC method with Email: {Email}, NewUsername: {NewUsername}, NewRole: {NewRole}", request.Email, request.NewUsername, request.NewRole);
            try
            {
                var reply = await _accountClient.UpdateUserAsync(request);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed with Status: {Status} and Detail: {Detail}", ex.Status.StatusCode, ex.Status.Detail);
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            _logger.LogInformation("Calling ChangePassword gRPC method with Email: {Email}", request.Email);
            try
            {
                var reply = await _accountClient.ChangePasswordAsync(request);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed with Status: {Status} and Detail: {Detail}", ex.Status.StatusCode, ex.Status.Detail);
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(PasswordResetRequest request)
        {
            _logger.LogInformation("Calling RequestPasswordReset gRPC method with Email: {Email}", request.Email);
            try
            {
                var reply = await _accountClient.RequestPasswordResetAsync(request);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed with Status: {Status} and Detail: {Detail}", ex.Status.StatusCode, ex.Status.Detail);
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            _logger.LogInformation("Calling ResetPassword gRPC method with Email: {Email}", request.Email);
            try
            {
                var reply = await _accountClient.ResetPasswordAsync(request);
                return Ok(reply);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed with Status: {Status} and Detail: {Detail}", ex.Status.StatusCode, ex.Status.Detail);
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
