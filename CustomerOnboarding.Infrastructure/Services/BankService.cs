using CustomerOnboarding.Core.DTOs;
using System.Net.Http.Json;

namespace CustomerOnboarding.Infrastructure.Services
{
    public class BankService : IBankService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BankService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<BankDto>> GetBanksAsync()
        {
            var client = _httpClientFactory.CreateClient("alat");
            var response = await client.GetAsync("merchant-onboarding/api/v1/banks");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch banks. Status: {response.StatusCode}");

            var banks = await response.Content.ReadFromJsonAsync<IEnumerable<BankDto>>();
            return banks ?? Enumerable.Empty<BankDto>();
        }
    }
}
