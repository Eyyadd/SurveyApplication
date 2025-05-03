using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.Auth.Login;
using Survey.Infrastructure.DTOs.Auth.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.IService
{
    public interface IAuthService
    {
        Task<Result<LoginResponse>> LoginAsync(string Email, string Password, CancellationToken cancellation = default);
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellation = default);
        Task<Result<LoginResponse>> GetRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellation = default);
        Task<Result> RevokeRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellation = default);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellation = default);
        Task<Result> ResendMailVerification(string email, CancellationToken cancellation = default);

    }
}
