using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BosquesNalcahue.Application.Services
{
    public class JwtService(UserManager<WebPortalUser> userManager, IOptions<JwtSettings> settings)
    {
        private readonly UserManager<WebPortalUser> _userManager = userManager;
        private readonly JwtSettings jwtSettings = settings.Value;

        public string GenerateToken(WebPortalUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            var securityKey = Encoding.ASCII.GetBytes(jwtSettings.SecurityKey!);

            List<Claim> claims = GenerateClaims(user);

            SecurityTokenDescriptor tokenDescriptor = GenerateTokenDescriptor(claims, securityKey);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private List<Claim> GenerateClaims(WebPortalUser user) =>
            [
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.Name, user.UserName ?? ""),
                new(JwtRegisteredClaimNames.Aud, jwtSettings.ValidAudience ?? ""),
                new(JwtRegisteredClaimNames.Iss, jwtSettings.ValidIssuer ?? ""),
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
