using BosquesNalcahue.Application.Entities;
using Microsoft.AspNetCore.Identity;

namespace BosquesNalcahue.Application.Services
{
    public class IdentityService(RoleManager<IdentityRole> roleManager, UserManager<WebPortalUser> userManager)
    {
        private readonly UserManager<WebPortalUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task AddRolesToUserAsync(WebPortalUser user, IEnumerable<string>? roles)
        {
            if (roles is null)
            {
                await _userManager.AddToRolesAsync(user, ["User"]);
            }
            else
            {
                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }

        public async Task<bool> CheckPasswordAsync(WebPortalUser user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        public async Task<IdentityResult> CreateUserAsync(WebPortalUser user, string password) =>
            await _userManager.CreateAsync(user, password);

        public async Task<WebPortalUser?> FindByEmailAsync(string email) =>
            await _userManager.FindByEmailAsync(email);
    }
}
