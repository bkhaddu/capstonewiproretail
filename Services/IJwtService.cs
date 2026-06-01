using RetailOptimizationPlatform.Models;

namespace RetailOptimizationPlatform.Services
{
    public interface IJwtService
    {
        string GenerateToken(AppUser user);
    }
}