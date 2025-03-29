using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Infrastructure.DTOs.Auth.Login;
using Survey.Infrastructure.DTOs.Auth.Register;
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
            var Result = await _AuthService.LoginAsync(request.Email, request.Password, cancellationToken);

            return Result is not null ? Ok(Result) : BadRequest("Invalid Mail Or Password");
        }

        [HttpPost(template: "Register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            var Result = await _AuthService.RegisterAsync(request, cancellationToken);

            return Result ? Ok(Result) : BadRequest("Sorry we can't create an accoun currently");
        }
    }
}
