using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BosquesNalcahue.Application.Services
{
    public class JwtService(UserManager<WebPortalUser> userManager, IOptions<JwtSettings> settings)
    {
        private readonly UserManager<WebPortalUser> _userManager = userManager;
        private readonly JwtSettings jwtSettings = settings.Value;
        public readonly int RefreshTokenDurationInDays = 1;

        public (string Jwt, DateTime ValidTo) GenerateJwtToken(WebPortalUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            var securityKey = Encoding.ASCII.GetBytes(jwtSettings.SecurityKey!);

            List<Claim> claims = GenerateClaims(user);

            SecurityTokenDescriptor tokenDescriptor = GenerateTokenDescriptor(claims, securityKey);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), token.ValidTo);
        }

        public string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token) =>
            new JwtSecurityTokenHandler().ValidateToken(token, GetTokenValidationParameters(), out _);

        private TokenValidationParameters GetTokenValidationParameters() =>
            new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings?.ValidIssuer,
                ValidAudience = jwtSettings?.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.SecurityKey ?? ""))
            };

        private List<Claim> GenerateClaims(WebPortalUser user) =>
            [
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.Name, user.UserName ?? ""),
                new(JwtRegisteredClaimNames.Aud, jwtSettings.ValidAudience ?? ""),
                new(JwtRegisteredClaimNames.Iss, jwtSettings.ValidIssuer ?? ""),
                new(JwtRegisteredClaimNames.Exp, jwtSettings.TokenLifeTimeInHours.ToString()),
                new(ClaimTypes.Name, user.UserName ?? ""),
                new("IsAdmin", user.IsAdmin.ToString())
            ];
        

        private SecurityTokenDescriptor GenerateTokenDescriptor(List<Claim> claims, byte[] key) =>
            new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(jwtSettings.TokenLifeTimeInHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };
        
        
    }
}
