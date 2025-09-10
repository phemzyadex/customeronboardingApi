using CustomerOnboarding.Core.DTOs;

namespace CustomerOnboarding.Infrastructure.Services
{
    public interface IBankService
    {
        Task<IEnumerable<BankDto>> GetBanksAsync();
    }
}
