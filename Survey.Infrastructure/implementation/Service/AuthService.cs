﻿using Hangfire;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Survey.Domain.Entities;
using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.Auth;
using Survey.Infrastructure.DTOs.Auth.Login;
using Survey.Infrastructure.DTOs.Auth.Register;
using Survey.Infrastructure.Errors;
using Survey.Infrastructure.IService;
using Survey.Infrastructure.Template;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;


namespace Survey.Infrastructure.implementation.Service
{
    public class AuthService(UserManager<ApplicationUser> userManager,
        IOptions<JwtOptions> JwtOptions,
        ILogger<AuthService> logger,
        IEmailSender Email,
        IHttpContextAccessor contextAccessor) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _UserManager = userManager;
        private readonly ILogger<AuthService> _Logger = logger;
        private readonly IEmailSender _Email = Email;
        private readonly IHttpContextAccessor _ContextAccessor = contextAccessor;
        private readonly JwtOptions _JwtOptions = JwtOptions.Value;
        private readonly int _refreshTokenExpiryTime = 14;

        public async Task<Result<LoginResponse>> LoginAsync(string Email, string Password, CancellationToken cancellation = default)
        {
            var userIsExist = await _UserManager.FindByEmailAsync(Email);
            if (userIsExist is null)
                return Result.Failure<LoginResponse>(AuthErrors.InvalidLogin);

            var checkPassword = await _UserManager.CheckPasswordAsync(userIsExist, Password);
            if (!checkPassword)
                return Result.Failure<LoginResponse>(AuthErrors.InvalidLogin);

            if (!userIsExist.EmailConfirmed)
                return Result.Failure<LoginResponse>(AuthErrors.EmailNotConfirmed);


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


            return Result.Success(reponse);

        }


        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellation = default)
        {
            var userIsExist = await _UserManager.FindByEmailAsync(request.Email);

            if (userIsExist is not null)
                return Result.Failure(AuthErrors.UserAlreadyExists);

            var user = request.Adapt<ApplicationUser>();
            var result = await _UserManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var code = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _Logger.LogInformation(message: code);

                //TODO: Send Mail Verifcation
                await SendMailVerification(user, code);
                return Result.Success();
            }

            return Result.Failure(AuthErrors.InvalidRegister);
        }
        public async Task<Result<LoginResponse>> GetRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellation = default)
        {
            var userId = ValidateToken(token);
            if (userId is null)
                return Result.Failure<LoginResponse>(AuthErrors.InvalidToken);

            var user = await _UserManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure<LoginResponse>(AuthErrors.UserNotFound);

            var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == RefreshToken && x.IsActive);
            if (userRefreshToken is null)
                return Result.Failure<LoginResponse>(AuthErrors.RefreshTokenNotFound);

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

            return Result.Success(response);
        }


        public async Task<Result> RevokeRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellation = default)
        {
            var userId = ValidateToken(token);

            if (userId is null)
                return Result.Failure(AuthErrors.InvalidToken);

            var user = await _UserManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure(AuthErrors.UserNotFound);

            var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == RefreshToken && x.IsActive);
            if (userRefreshToken is null)
                return Result.Failure(AuthErrors.RefreshTokenNotFound);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _UserManager.UpdateAsync(user);

            return Result.Success();
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellation = default)
        {
            var user = await _UserManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result.Failure(AuthErrors.UserNotFound);
            if (user.EmailConfirmed)
                return Result.Failure(AuthErrors.EmailAlreadyConfirmed);
            var decodedCode = request.Code;
            try
            {
                decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(decodedCode));

            }
            catch (Exception)
            {
                return Result.Failure(AuthErrors.InvalidCode);
            }
            var result = await _UserManager.ConfirmEmailAsync(user, decodedCode);

            if (result.Succeeded)
                return Result.Success();
            var error = result.Errors.FirstOrDefault();
            return Result.Failure(new Error(error.Code, error.Description));
        }

        public async Task<Result> ResendMailVerification(string Email, CancellationToken cancellation = default)
        {
            var user = await _UserManager.FindByEmailAsync(Email);
            if (user is null)
                return Result.Failure(AuthErrors.UserNotFound);
            if (user.EmailConfirmed)
                return Result.Failure(AuthErrors.EmailAlreadyConfirmed);

            var code = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _Logger.LogInformation(message: code);


            //TODO: Send Code to Email
            await SendMailVerification(user, code);
            return Result.Success();
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

        private async Task SendMailVerification(ApplicationUser user, string code)
        {
            var origin = _ContextAccessor.HttpContext?.Request.Headers.Origin;
            var dict = new Dictionary<string, string>
                {
                    {
                        "{{UserName}}" , user.FirstName + " " + user.LastName
                    },
                    {
                        "{{VerificationLink}}",$"{origin}/auth/Confirm-Email?userId=${user.Id}&code=${code}"
                    },
                    {
                        "{{VerificationCode}}",$"{code}"
                    },
                    {
                        "{{CurrentYear}}",$"{DateOnly.FromDateTime(DateTime.Now).Year}"
                    }
                };
            var emailBody = EmailBodyBuilder.GenerateBody("EmailVerification", dict);
            BackgroundJob.Enqueue (() => _Email.SendEmailAsync(user.Email!, "SurveyBasket - Confirmation Mail ✅🤗", emailBody));

            await Task.CompletedTask;
        }


    }
}
