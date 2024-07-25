using BosquesNalcahue.Application.Entities;
using Microsoft.AspNetCore.Identity;

namespace BosquesNalcahue.Application.Services
{
    public class IdentityService(RoleManager<IdentityRole> roleManager, UserManager<WebPortalUser> userManager)
    {
        private readonly UserManager<WebPortalUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<bool> CheckPasswordAsync(WebPortalUser user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        public async Task<IdentityResult> CreateUserAsync(WebPortalUser user, string password) =>
            await _userManager.CreateAsync(user, password);

        public async Task<WebPortalUser?> FindByEmailAsync(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<WebPortalUser?> FindByNameAsync(string name) =>
            await _userManager.FindByNameAsync(name);

        public async Task<IdentityResult> UpdateUserWithRefreshToken(WebPortalUser user, string refreshToken, int refreshTokenDuration)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenDuration);
            return await _userManager.UpdateAsync(user);
        }
    }
}
