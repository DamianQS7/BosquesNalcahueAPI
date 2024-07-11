using BosquesNalcahue.API.Mapping;
using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Services;
using BosquesNalcahue.Contracts.Requests;
using BosquesNalcahue.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BosquesNalcahue.API.Controllers
{
    [ApiController]
    public class IdentityController(IdentityService identityService, JwtService jwtService) : ControllerBase
    {
        private readonly IdentityService _identityService = identityService;
        private readonly JwtService _jwtService = jwtService;

        [HttpPost(Endpoints.Identity.Login)]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _identityService.FindByEmailAsync(request.Email);

            if (user is null)
                return Unauthorized(new AuthResponse { 
                    Success = false, 
                    Message = "No user was found using with this email." 
                });

            var result = await _identityService.CheckPasswordAsync(user, request.Password);

            if (!result)
                return Unauthorized(new AuthResponse { Success = false, Message = "Invalid password." });

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                Token = _jwtService.GenerateToken(user)
            });
        }

        [HttpPost(Endpoints.Identity.Register)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            WebPortalUser user = request.MapToAppUser();

            var result = await _identityService.CreateUserAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var response = new AuthResponse
            {
                Success = true,
                Message = "Account created successfully"
            };

            return Ok(response);
        }
    }
}
