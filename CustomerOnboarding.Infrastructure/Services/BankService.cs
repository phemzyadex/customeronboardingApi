using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
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

        public async Task<BankApiResponse> GetBanksAsync()
        {
            var client = _httpClientFactory.CreateClient("alat");

            var response = await client.GetAsync("alat-test/api/Shared/GetAllBanks");

            if (!response.IsSuccessStatusCode)
            {
                return new BankApiResponse
                {
                    HasError = true,
                    ErrorMessage = $"Failed to fetch banks. Status: {response.StatusCode}",
                    TimeGenerated = DateTime.UtcNow
                };
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<BankApiResponse>();

            if (apiResponse == null)
            {
                return new BankApiResponse
                {
                    HasError = true,
                    ErrorMessage = "Invalid response from bank API.",
                    TimeGenerated = DateTime.UtcNow
                };
            }
            Console.WriteLine(apiResponse);
            return apiResponse;
        }
    }
}
