using System.ComponentModel.DataAnnotations;

namespace BosquesNalcahue.Contracts.Requests
{
    public class RegisterUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required] 
        public string UserName { get; set; } = string.Empty;

        [Required]
        public bool IsAdmin { get; set; } = false;
    }
}
