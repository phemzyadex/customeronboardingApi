using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;

namespace CustomerOnboarding.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<ApiResponse<CustomerResponse>> OnboardAsync(OnboardCustomerDto dto);
        Task<ApiResponse<bool>> VerifyPhoneAsync(VerifyPhoneDto dto);
        Task<ApiResponse<IEnumerable<CustomerDto>>> GetAllAsync();
        Task<ApiResponse<bool>> SoftDeleteCustomerAsync(Guid customerId);
    }
}
