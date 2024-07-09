using BosquesNalcahue.API.Mapping;
using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Contracts.Requests;
using BosquesNalcahue.Contracts.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BosquesNalcahue.API.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<WebPortalUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityController(UserManager<WebPortalUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost(Endpoints.Identity.Register)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            WebPortalUser user = request.MapToAppUser();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (request.Roles is null)
            {
                await _userManager.AddToRolesAsync(user, ["User"]);
            }
            else
            {
                foreach (var role in request.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            var response = new AuthResponse
            {
                Success = true,
                Message = "Account created successfully"
            };

            return Ok(response);
        }
    }
}
