using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Survey.Domain.Entities;
using Survey.Infrastructure.DTOs.Auth.Login;
using Survey.Infrastructure.DTOs.Auth.Register;
using Survey.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.implementation.Service
{
    public class AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _UserManager = userManager;

        public async Task<LoginResponse?> LoginAsync(string Email, string Password, CancellationToken cancellation = default)
        {
            var UserIsExist = await _UserManager.FindByEmailAsync(Email);
            if (UserIsExist is null)
                return null!;

            var CheckPassword = await _UserManager.CheckPasswordAsync(UserIsExist, Password);
            if (!CheckPassword)
                return null!;

            //Generate Token

            var token = GenerateToken(UserIsExist);
            var reponse = new LoginResponse
            {
                Email = Email,
                FirstName = UserIsExist.FirstName,
                LastName = UserIsExist.LastName,
                Id = UserIsExist.Id,
                Token = token.validToken,
                ExpireIn = token.expires
            };
            return reponse;

        }


        public async Task<bool> RegisterAsync(RegisterRequest request, CancellationToken cancellation = default)
        {
            var userIsExist = await _UserManager.FindByEmailAsync(request.Email);

            if (userIsExist is not null)
                return false;

            var user = request.Adapt<ApplicationUser>();
            var result = await _UserManager.CreateAsync(user,request.Password);

            if(result.Succeeded)
                return true;

            return false;
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

            var Key = configuration["JwtOptions:securityKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key!));
            var singingCrednatials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var ExpiresIn = configuration.GetSection("JwtOptions:expireIn");

            var token = new JwtSecurityToken(
                issuer: configuration["JwtOptions:issuer"],
                audience: configuration["JwtOptions:audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(40),
                signingCredentials: singingCrednatials

            );

            var validToken = new JwtSecurityTokenHandler().WriteToken(token);

            return (validToken, 40);


        }
    }
}
