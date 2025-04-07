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
        Task<LoginResponse?> LoginAsync(string Email, string Password, CancellationToken cancellation = default);
        Task<bool> RegisterAsync(RegisterRequest request, CancellationToken cancellation = default);
        Task<LoginResponse?> GetRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellation = default);
        Task<bool> RevokeRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellation = default);
    }
}
