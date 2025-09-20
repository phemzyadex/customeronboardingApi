using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Models;

namespace CustomerOnboarding.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AppUser?> RegisterAsync(string username, string password, string role);
        Task<AuthResponseDto?> LoginAsync(string username, string password);
        Task<List<AppUser>> GetAllUsersAsync();
    }
}
