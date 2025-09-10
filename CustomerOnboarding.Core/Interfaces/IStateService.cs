using CustomerOnboarding.Core.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Core.Interfaces
{
    public interface IStateService
    {
        Task<StateResponseDto> AddStateAsync(string name);
        Task<List<StateResponseDto>> GetAllStatesAsync();
    }
}
