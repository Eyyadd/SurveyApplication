using Survey.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Errors
{
    public static class AuthErrors
    {
        public static Error InvalidLogin => new Error("Invalid Login", "Invalid login attempt. Please check your credentials and try again.");
        public static Error UserAlreadyExists => new Error("User Already Exists", "A user with this email already exists. Please use a different email address.");
        public static Error UserNotFound => new Error("User Not Found", "User not found. Please check the email address and try again.");
        public static Error InvalidToken => new Error("Invalid Token", "The provided token is invalid or has expired. Please log in again.");
        public static Error TokenRevoked => new Error("Token Revoked", "The provided token has been revoked. Please log in again.");
        public static Error RefreshTokenNotFound => new Error("Refresh Token Not Found", "The provided refresh token was not found. Please log in again.");
        public static Error RefreshTokenExpired => new Error("Refresh Token Expired", "The provided refresh token has expired. Please log in again.");
        public static Error InvalidRegister => new Error("Invalid Register", "Invalid registration attempt. Please check your details and try again.");

    }
}
