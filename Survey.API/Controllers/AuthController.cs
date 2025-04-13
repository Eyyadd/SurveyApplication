using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.Auth.Login;
using Survey.Infrastructure.DTOs.Auth.Refresh_Token;
using Survey.Infrastructure.DTOs.Auth.Register;
using Survey.Infrastructure.Extensions;
using Survey.Infrastructure.IService;

namespace Survey.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _AuthService = authService;

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request,CancellationToken cancellationToken)
        {
            var result = await _AuthService.LoginAsync(request.Email, request.Password, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.StandardError(StatusCodes.Status400BadRequest);
        }

        [HttpPost(template: "Register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _AuthService.RegisterAsync(request, cancellationToken);

            return result.IsSuccess ? Ok() : result.StandardError(StatusCodes.Status400BadRequest);
        }

        [HttpPost(template: "RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _AuthService.GetRefreshTokenAsync(request.Token,request.RefreshToken, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.StandardError(StatusCodes.Status400BadRequest);
        }

        [HttpPost(template: "Revoke-RefreshToken")]
        public async Task<IActionResult> RevokeRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _AuthService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return result.IsSuccess ? Ok() : result.StandardError(StatusCodes.Status400BadRequest);
        }
    }
}
