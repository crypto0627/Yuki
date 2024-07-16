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
            _logger.LogInformation("Calling Register gRPC method with Username: {Username} and Email: {Email}", request.Username, request.Email);
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
    }
}
