using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Survey.Domain.Entities;
using Survey.Infrastructure.DTOs.Auth;
using Survey.Infrastructure.DTOs.Auth.Login;
using Survey.Infrastructure.DTOs.Auth.Register;
using Survey.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.implementation.Service
{
    public class AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtOptions> JwtOptions) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _UserManager = userManager;
        private readonly JwtOptions _JwtOptions = JwtOptions.Value;
        private readonly int _refreshTokenExpiryTime = 14;

        public async Task<LoginResponse?> LoginAsync(string Email, string Password, CancellationToken cancellation = default)
        {
            var userIsExist = await _UserManager.FindByEmailAsync(Email);
            if (userIsExist is null)
                return null!;

            var checkPassword = await _UserManager.CheckPasswordAsync(userIsExist, Password);
            if (!checkPassword)
                return null!;

            //Generate Token
            var token = GenerateToken(userIsExist);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryTime);

            userIsExist.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiryTime
            });

            await _UserManager.UpdateAsync(userIsExist);

            var reponse = new LoginResponse
            {
                Email = Email,
                FirstName = userIsExist.FirstName,
                LastName = userIsExist.LastName,
                Id = userIsExist.Id,
                Token = token.validToken,
                ExpireIn = token.expires,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshTokenExpiryTime
            };
            return reponse;

        }


        public async Task<bool> RegisterAsync(RegisterRequest request, CancellationToken cancellation = default)
        {
            var userIsExist = await _UserManager.FindByEmailAsync(request.Email);

            if (userIsExist is not null)
                return false;

            var user = request.Adapt<ApplicationUser>();
            var result = await _UserManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
                return true;

            return false;
        }
        public async Task<LoginResponse?> GetRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellation = default)
        {
            var userId = ValidateToken(token);
            if (userId == null)
                return null!;

            var user = await _UserManager.FindByIdAsync(userId);
            if (user == null)
                return null!;

            var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == RefreshToken && x.IsActive);
            if (userRefreshToken == null)
                return null!;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var (newtoken, expires) = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            var newRefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryTime);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = newRefreshTokenExpiryTime
            });

            await _UserManager.UpdateAsync(user);

            var response = new LoginResponse
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = newtoken,
                ExpireIn = expires,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiryTime = newRefreshTokenExpiryTime
            };

            return response;
        }


        public async Task<bool> RevokeRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellation = default)
        {
            var userId = ValidateToken(token);

            if (userId is null)
                return false;

            var user = await _UserManager.FindByIdAsync(userId);
            if (user is null)
                return false;

            var userRefreshToken =  user.RefreshTokens.FirstOrDefault(x => x.Token == RefreshToken && x.IsActive);
            if (userRefreshToken is null)
                return false;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _UserManager.UpdateAsync(user);

            return true;
        }

        private (string validToken, int expires) GenerateToken(ApplicationUser user)
        {
            Claim[] claims = [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email , user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName,user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName,user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            ];

            var Key = _JwtOptions.Key;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key!));
            var singingCrednatials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var ExpiresIn = _JwtOptions.ExpireIn;

            var token = new JwtSecurityToken(
                issuer: _JwtOptions.Issuer,
                audience: _JwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(ExpiresIn),
                signingCredentials: singingCrednatials

            );

            var validToken = new JwtSecurityTokenHandler().WriteToken(token);

            return (validToken, ExpiresIn);


        }

        public static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtOptions.Key!));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        
    }
}
