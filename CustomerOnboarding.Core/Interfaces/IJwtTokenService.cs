using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Models;

namespace CustomerOnboarding.Core.Interfaces
{
    public interface IJwtTokenService
    {
        AuthResponseDto GenerateToken(AppUser user);
    }
}
