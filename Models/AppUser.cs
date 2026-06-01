using System.ComponentModel.DataAnnotations;

namespace RetailOptimizationPlatform.Models
{
    public class AppUser
    {
        public int AppUserId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer";
    }
}