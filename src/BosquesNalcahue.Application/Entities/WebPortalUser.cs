using Microsoft.AspNetCore.Identity;

namespace BosquesNalcahue.Application.Entities
{
    public class WebPortalUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
