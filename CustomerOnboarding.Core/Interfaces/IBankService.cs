using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;

namespace CustomerOnboarding.Infrastructure.Services
{
    public interface IBankService
    {
        Task<BankApiResponse> GetBanksAsync();
    }
}
